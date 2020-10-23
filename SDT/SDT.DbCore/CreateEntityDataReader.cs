using System;
using System.Collections;
using System.Data.Common;

namespace SDT.DbCore
{
    internal class CreateEntityDataReader: DbDataReader
    {
        public CreateEntityDataReader(DbDataReader originalDataReader) => OriginalDataReader = originalDataReader;

        private DbDataReader OriginalDataReader { get; }

        public override object this[string name] => OriginalDataReader[name];

        public override object this[int ordinal] => OriginalDataReader[ordinal];

        public override int Depth => OriginalDataReader.Depth;

        public override int FieldCount => OriginalDataReader.FieldCount;

        public override bool HasRows => OriginalDataReader.HasRows;

        public override bool IsClosed => OriginalDataReader.IsClosed;

        public override int RecordsAffected => OriginalDataReader.RecordsAffected;

        public override bool GetBoolean(int ordinal) => OriginalDataReader.GetBoolean(ordinal);

        public override byte GetByte(int ordinal) => OriginalDataReader.GetByte(ordinal);

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => OriginalDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

        public override char GetChar(int ordinal) => OriginalDataReader.GetChar(ordinal);

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => OriginalDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

        public override string GetDataTypeName(int ordinal) => OriginalDataReader.GetDataTypeName(ordinal);

        public override DateTime GetDateTime(int ordinal) => OriginalDataReader.GetDateTime(ordinal);

        public override decimal GetDecimal(int ordinal) => OriginalDataReader.GetDecimal(ordinal);

        public override double GetDouble(int ordinal) => OriginalDataReader.GetDouble(ordinal);

        public override IEnumerator GetEnumerator() => OriginalDataReader.GetEnumerator();

        public override Type GetFieldType(int ordinal) => OriginalDataReader.GetFieldType(ordinal);

        public override float GetFloat(int ordinal) => OriginalDataReader.GetFloat(ordinal);

        public override Guid GetGuid(int ordinal) => OriginalDataReader.GetGuid(ordinal);

        public override short GetInt16(int ordinal) => OriginalDataReader.GetInt16(ordinal);

        public override int GetInt32(int ordinal) => OriginalDataReader.GetInt32(ordinal);

        public override long GetInt64(int ordinal) => OriginalDataReader.GetInt64(ordinal);

        public override string GetName(int ordinal) => OriginalDataReader.GetName(ordinal);

        public override int GetOrdinal(string name) => OriginalDataReader.GetOrdinal(name);


        public override string GetString(int ordinal) => OriginalDataReader.GetString(ordinal);

        public override object GetValue(int ordinal) => OriginalDataReader.GetValue(ordinal);

        public override int GetValues(object[] values) => OriginalDataReader.GetValues(values);

        public override bool IsDBNull(int ordinal) => OriginalDataReader.IsDBNull(ordinal);

        public override bool NextResult() => OriginalDataReader.NextResult();

        public override bool Read() => OriginalDataReader.Read();

        public override T GetFieldValue<T>(int ordinal)
        {
            var value = GetValue(ordinal);

            if (typeof(T) == typeof(DateTimeOffset) && value is DateTime valueDateTime)
            {
                value = new DateTimeOffset(valueDateTime);
            }
            else if (typeof(T) == typeof(Guid) && value is byte[] valueByteArray && valueByteArray.Length == 16)
            {
                value = new Guid(valueByteArray);
            }

            return (T)value;
        }
    }
}
