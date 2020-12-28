using Elasticsearch.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CPC.Logger
{
    public class EsClient
    {
        #region Members
        private readonly Lazy<IElasticLowLevelClient> _client;
        private readonly ConcurrentDictionary<string, Lazy<IElasticLowLevelClient>> _clientPools = new ConcurrentDictionary<string, Lazy<IElasticLowLevelClient>>();
        #endregion

        public EsClient(IEnumerable<Uri> uris)
        {
            var nodes = uris.OrderBy(t => t);
            var key = string.Join("|", nodes);
            _client = _clientPools.GetOrAdd(key, _ => new Lazy<IElasticLowLevelClient>(() =>
            {
                IConnectionPool connectionPool = new StaticConnectionPool(nodes);
                var config = new ConnectionConfiguration(connectionPool);
                return new ElasticLowLevelClient(config);
            }));
        }

        public bool SendBatch(string index, string type, List<EsBody> bodies)
        {
            index = index?.ToLowerInvariant();
            type = type?.ToLowerInvariant();
            var payloads = new List<object>(bodies.Count * 2);

            foreach (var body in bodies)
            {
                body.Document["@timestamp"] = DateTimeUtility.Now;
                body.Document["time"] = body.Time;
                var documentInfo = new { index = new { _index = index, _type = type } };
                payloads.AddRange(new object[] { documentInfo, body.Document });
            }

            try
            {
                var post = PostData.MultiJson(payloads);
                var result = _client.Value.Bulk<BytesResponse>(post);
                payloads.Clear();
                return result.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
