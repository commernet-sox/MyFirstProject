using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CPC.Logger
{
    public class EsPool
    {
        #region Members
        private static readonly List<EsIndex> _esIndexPools = new List<EsIndex>();
        private static EsClient _esClient;
        private static readonly List<(string OrigIndex, string Format, string Index)> _esFormats = new List<(string OrigIndex, string Format, string Index)>();
        private static readonly BlockingCollection<EsExternal> _esPools = new BlockingCollection<EsExternal>();
        public static Func<string, string, string> FormatIndex { get; set; }
        public static Action<EsExternal> Process { get; set; }
        #endregion

        #region Methods

        public static void Write(EsExternal body)
        {
            Process?.Invoke(body);
            _esPools.Add(body);
        }

        private static void AllocationAsync() => Task.Factory.StartNew(() =>
                                               {
                                                   while (!_esPools.IsCompleted)
                                                   {
                                                       var item = _esPools.Take();
                                                       var index = CallFormatIndex(item.Index, item.SuffixFormat);
                                                       var esIndex = _esIndexPools.FirstOrDefault(t => t.Index == index);
                                                       if (esIndex == null)
                                                       {
                                                           if (!item.SuffixFormat.IsNull())
                                                           {
                                                               var expFormat = _esFormats.FirstOrDefault(t => t.OrigIndex == item.Index && t.Format == item.SuffixFormat);

                                                               if (!expFormat.Index.IsNull())
                                                               {
                                                                   Abort(expFormat.Index);
                                                                   _esFormats.Remove(expFormat);
                                                               }

                                                               _esFormats.Add((item.Index, item.SuffixFormat, index));
                                                           }

                                                           esIndex = new EsIndex(_esClient, index, item.Type);
                                                           _esIndexPools.Add(esIndex);
                                                       }
                                                       esIndex.Add(new EsBody() { Document = item.Document, Time = item.Time });
                                                   }
                                               }, TaskCreationOptions.LongRunning);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Initialize(IEnumerable<Uri> uris)
        {
            if (_esClient != null)
            {
                throw new InvalidOperationException("此方法已被调用");
            }

            _esClient = new EsClient(uris);
            AllocationAsync();
        }

        public static void Dispose()
        {
            if (_esClient != null)
            {
                _esPools.CompleteAdding();
            }
        }

        private static bool Abort(string index)
        {
            var esIndex = _esIndexPools.FirstOrDefault(t => t.Index == index);
            if (esIndex != null)
            {
                esIndex.Abort();
                _esIndexPools.Remove(esIndex);
                return true;
            }
            return false;
        }

        private static string CallFormatIndex(string index, string suffixFormat)
        {
            if (FormatIndex != null)
            {
                return FormatIndex.Invoke(index, suffixFormat);
            }

            if (suffixFormat.IsNull())
            {
                return index;
            }

            index += DateTimeUtility.Now.ToString(suffixFormat);
            return index;
        }
        #endregion
    }
}
