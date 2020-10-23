using SDT.BaseTool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SDT.DbCore
{
    public class SqlResult
    {
        private readonly DbDataReader _reader;

        public SqlResult(DbDataReader reader) => _reader = reader;

        public IList<T> ReadToList<T>() => MapToList<T>(_reader);

        public T? ReadToValue<T>() where T : struct => MapToValue<T>(_reader);

        public Task<bool> NextResultAsync() => _reader.NextResultAsync();

        public Task<bool> NextResultAsync(CancellationToken ct) => _reader.NextResultAsync(ct);

        public bool NextResult() => _reader.NextResult();

        /// <summary>
        /// Retrieves the column values from the stored procedure and maps them to <typeparamref name="T"/>'s properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private IList<T> MapToList<T>(DbDataReader dr)
        {
            var list = new List<T>();

            var colMapping = dr.GetColumnSchema()
                .ToDictionary(key => key.ColumnName.ToLowerInvariant());

            if (dr.HasRows)
            {
                var plist = new List<(string ColumnName, PropertyInfo Property)>();
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    if (!prop.CanWrite || prop.GetCustomAttribute<NotMappedAttribute>(false) != null)
                    {
                        continue;
                    }

                    var name = prop.Name.ToLowerInvariant();
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(true);
                    if (colAttr != null && !colAttr.Name.IsNull())
                    {
                        name = colAttr.Name.ToLowerInvariant();
                    }

                    if (!colMapping.ContainsKey(name))
                    {
                        continue;
                    }

                    plist.Add((name, prop));
                }

                while (dr.Read())
                {
                    var data = Activator.CreateInstance<T>();
                    foreach (var (columnName, prop) in plist)
                    {
                        var column = colMapping[columnName];
                        if (column?.ColumnOrdinal != null)
                        {
                            var val = dr.GetValue(column.ColumnOrdinal.Value);
                            prop.SetValue(data, val == DBNull.Value ? null : val);
                        }

                        Thread.Sleep(0);
                    }

                    list.Add(data);
                }
            }

            return list;
        }

        /// <summary>
        ///Attempts to read the first value of the first row of the resultset.
        /// </summary>
        private T? MapToValue<T>(DbDataReader dr) where T : struct
        {
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    return
                        dr.IsDBNull(0) ? new T?() : new T?(dr.GetFieldValue<T>(0));
                }
            }
            return new T?();
        }
    }
}
