using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace CPC.DbComponent
{
    public static class ParameterExtension
    {
        #region OleDbParameter
        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil) => new OleDbParameter();

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, object value) => new OleDbParameter(name, value);

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, OleDbType dataType) => new OleDbParameter(name, dataType);

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, OleDbType dataType, int size) => new OleDbParameter(name, dataType, size);

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, OleDbType dataType, int size, string srcColumn) => new OleDbParameter(name, dataType, size, srcColumn);

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, OleDbType dataType, int size, ParameterDirection
            direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value) => new OleDbParameter(name, dataType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);

        public static DbParameter NewOleDbParameter(this IDbUtil dbUtil, string name, OleDbType dbType, int size, ParameterDirection
            direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value) => new OleDbParameter(name, dbType, size, direction, precision, scale, sourceColumn, sourceVersion, sourceColumnNullMapping, value);

        #endregion

        #region SqlParameter
        public static DbParameter NewSqlParameter(this IDbUtil dbUtil) => new SqlParameter();

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, object value) => new SqlParameter(name, value);

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, SqlDbType dataType) => new SqlParameter(name, dataType);

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, SqlDbType dataType, int size) => new SqlParameter(name, dataType, size);

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, SqlDbType dataType, int size, string srcColumn) => new SqlParameter(name, dataType, size, srcColumn);

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, SqlDbType dataType, int size, ParameterDirection
            direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value) => new SqlParameter(name, dataType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);

        public static DbParameter NewSqlParameter(this IDbUtil dbUtil, string name, SqlDbType dbType, int size, ParameterDirection direction,
            byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping,
            object value, string xmlSchemaCollectionDatabase, string xmlSchemaCollectionOwningSchema, string xmlSchemaCollectionName) => new SqlParameter(name, dbType, size, direction, precision, scale, sourceColumn, sourceVersion, sourceColumnNullMapping, value,
                xmlSchemaCollectionDatabase, xmlSchemaCollectionOwningSchema, xmlSchemaCollectionName);
        #endregion

        #region OracleParameter
        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil)
        //{
        //    return new OracleParameter();
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, object value)
        //{
        //    return new OracleParameter(name, value);
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, OracleType oracleType)
        //{
        //    return new OracleParameter(name, oracleType);
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, OracleType oracleType, int size)
        //{
        //    return new OracleParameter(name, oracleType, size);
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, OracleType oracleType, int size, string srcColumn)
        //{
        //    return new OracleParameter(name, oracleType, size, srcColumn);
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, OracleType oracleType, int size, ParameterDirection direction, 
        //    string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value)
        //{
        //    return new OracleParameter(name, oracleType, size, direction, sourceColumn, sourceVersion, sourceColumnNullMapping, value);
        //}

        //public static DbParameter NewOracleParameter(this IDbUtil dbUtil, string name, OracleType oracleType, int size, ParameterDirection direction,
        //    bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
        //{
        //    return new OracleParameter(name, oracleType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);
        //}
        #endregion

        public static void SetProcedureParameter(this IDbUtil dbUtil, DataTable parameters, string parameterName, object parameterValue)
        {
            var flag = false;
            for (var i = 0; i < parameters.Rows.Count; i++)
            {
                var row = parameters.Rows[i];
                if (((row["ARGUMENT_NAME"].ToString().ToLower().Trim() == parameterName.ToLower().Trim()) || (("@" + row["ARGUMENT_NAME"].ToString().ToLower().Trim()) == parameterName.ToLower().Trim())) || (row["ARGUMENT_NAME"].ToString().ToLower().Trim() == ("@" + parameterName.ToLower().Trim())))
                {
                    flag = true;
                    row["PARMVALUE"] = parameterValue;
                }
            }
            if (!flag)
            {
                throw new Exception("存储过程未找到参数:" + parameterName);
            }
        }

        public static object GetProcedureParameter(this IDbUtil dbUtil, DataTable parameters, string parameterName)
        {
            object obj2 = null;
            for (var i = 0; i < parameters.Rows.Count; i++)
            {
                var row = parameters.Rows[i];
                if ((row["ARGUMENT_NAME"].ToString().ToLower() == parameterName.ToLower()) || (row["ARGUMENT_NAME"].ToString().ToLower() == ("@" + parameterName.ToLower())))
                {
                    obj2 = row["PARMVALUE"];
                }
            }
            if (obj2 == null)
            {
                throw new Exception("存储过程未找到参数:" + parameterName);
            }
            return obj2;
        }

    }
}
