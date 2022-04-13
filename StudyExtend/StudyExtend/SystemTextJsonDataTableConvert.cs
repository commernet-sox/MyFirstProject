using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudyExtend
{
    public class SystemTextJsonDataTableConvert<T> : JsonConverter<T>
        where T : DataTable
    {
        //public static void DTConvert()
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Name");
        //    dataTable.Columns.Add("Age");
        //    dataTable.Rows.Add("小王",11);
        //    dataTable.Rows.Add("Description",22);
        //    //var res = JsonSerializer.Serialize(dataTable);
        //    var res = JsonConvert.SerializeObject(dataTable);
        //    Console.WriteLine(res);
        //}
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            var dt = new DataTable();
            var list = new List<object>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    dt.Rows.Add(list.ToArray());
                    list.Clear();
                    //return (T)dt;
                }
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return (T)dt;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {

                    string propertyName = reader.GetString();

                    reader.Read();

                    switch (reader.TokenType)
                    {
                        case JsonTokenType.Number:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(decimal)));
                            }

                            list.Add(reader.GetDecimal());
                            break;
                        case JsonTokenType.String:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(string)));
                            }

                            list.Add(reader.GetString());
                            break;
                        case JsonTokenType.True:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(bool)));
                            }
                            list.Add(reader.GetBoolean());
                            break;
                        case JsonTokenType.False:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(bool)));
                            }
                            list.Add(reader.GetBoolean());
                            break;
                        case JsonTokenType.None:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(string)));
                            }
                            list.Add("");
                            break;
                        case JsonTokenType.Null:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(Nullable)));
                            }
                            list.Add(null);
                            break;
                        default:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(object)));
                            }
                            list.Add(JsonDocument.ParseValue(ref reader).RootElement.Clone());

                            break;
                    }

                }
            }
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (DataRow row in value.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn col in value.Columns)
                {
                    var t = col.DataType;

                    switch (Type.GetTypeCode(t))
                    {
                        case TypeCode.String:
                            writer.WriteString(col.ColumnName, row[col].ToString());
                            break;
                        case TypeCode.Boolean:
                            writer.WriteBoolean(col.ColumnName, (bool)row[col]);
                            break;
                        case TypeCode.Int32:
                            writer.WriteNumber(col.ColumnName, (int)row[col]);
                            break;
                        case TypeCode.DateTime:
                            writer.WriteString(col.ColumnName, row[col].ToString());
                            break;
                        case TypeCode.Decimal:
                            writer.WriteNumber(col.ColumnName, (decimal)row[col]);
                            break;
                        case TypeCode.Single:
                            writer.WriteNumber(col.ColumnName, (Single)row[col]);
                            break;
                        case TypeCode.Double:
                            writer.WriteNumber(col.ColumnName, (double)row[col]);
                            break;
                        case TypeCode.Char:
                            writer.WriteNumber(col.ColumnName, (char)row[col]);
                            break;
                        case TypeCode.Byte:
                            writer.WriteNumber(col.ColumnName, (byte)row[col]);
                            break;
                        case TypeCode.DBNull:
                            writer.WriteNull(col.ColumnName);
                            break;
                        case TypeCode.Empty:
                            writer.WriteString(col.ColumnName,"");
                            break;
                        case TypeCode.Object:
                            var type = row[col].GetType();
                            writer.WritePropertyName(col.ColumnName);
                            writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(row[col], type, options));
                            break;
                            
                    }

                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }

    public class DataSetConvert : JsonConverter<DataSet>
    {
        public override DataSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            
            DataSet dataSet=new DataSet();
            var dt = new DataTable();
            var list = new List<object>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    reader.Read();
                    //dt.Clear();
                }
                if (reader.TokenType == JsonTokenType.EndObject && list.Count == 0 && dt.Rows.Count == 0)
                {
                    return dataSet;
                }
                if (reader.TokenType == JsonTokenType.EndObject && list.Count != 0)
                {
                    dt.Rows.Add(list.ToArray());
                    list.Clear();
                    //return (T)dt;
                }
                if (reader.TokenType == JsonTokenType.EndArray && dt.Rows.Count != 0)
                {
                    dataSet.Merge(dt);
                    dt.Columns.Clear();
                    dt.TableName = "";
                    dt.Clear();
                    //return dataSet;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {

                    string propertyName = reader.GetString();

                    reader.Read();

                    switch (reader.TokenType)
                    {
                        //case JsonTokenType.StartArray:
                        //    if (dt.TableName != "")
                        //    {
                        //        dt.TableName = propertyName;
                        //    }
                        //    else
                        //    {
                        //        if (!dt.Columns.Contains(propertyName))
                        //        {
                        //            dt.Columns.Add(new DataColumn(propertyName, typeof(Array)));
                        //        }
                        //        reader.
                        //    }
                        //    break;
                        case JsonTokenType.Number:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(decimal)));
                            }

                            list.Add(reader.GetDecimal());
                            break;
                        case JsonTokenType.String:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(string)));
                            }

                            list.Add(reader.GetString());
                            break;
                        case JsonTokenType.True:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(bool)));
                            }
                            list.Add(reader.GetBoolean());
                            break;
                        case JsonTokenType.False:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(bool)));
                            }
                            list.Add(reader.GetBoolean());
                            break;
                        case JsonTokenType.None:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(string)));
                            }
                            list.Add("");
                            break;
                        case JsonTokenType.Null:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(Nullable)));
                            }
                            list.Add(null);
                            break;
                        default:
                            if (dt.TableName == "")
                            {
                                dt.TableName = propertyName;
                            }
                            else
                            {
                                if (!dt.Columns.Contains(propertyName))
                                {
                                    dt.Columns.Add(new DataColumn(propertyName, typeof(object)));
                                }
                                list.Add(JsonDocument.ParseValue(ref reader).RootElement.Clone());
                            }
                            
                            break;
                    }

                }
            }
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DataSet value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (DataTable dt in value.Tables)
            {
                writer.WritePropertyName(dt.TableName);
                writer.WriteStartArray();
                foreach (DataRow row in dt.Rows)
                {
                    writer.WriteStartObject();
                    foreach (DataColumn col in dt.Columns)
                    {
                        var t = col.DataType;

                        switch (Type.GetTypeCode(t))
                        {
                            case TypeCode.String:
                                writer.WriteString(col.ColumnName, row[col].ToString());
                                break;
                            case TypeCode.Boolean:
                                writer.WriteBoolean(col.ColumnName, (bool)row[col]);
                                break;
                            case TypeCode.Int32:
                                writer.WriteNumber(col.ColumnName, (int)row[col]);
                                break;
                            case TypeCode.DateTime:
                                writer.WriteString(col.ColumnName, row[col].ToString());
                                break;
                            case TypeCode.Decimal:
                                writer.WriteNumber(col.ColumnName, (decimal)row[col]);
                                break;
                            case TypeCode.Single:
                                writer.WriteNumber(col.ColumnName, (Single)row[col]);
                                break;
                            case TypeCode.Double:
                                writer.WriteNumber(col.ColumnName, (double)row[col]);
                                break;
                            case TypeCode.Char:
                                writer.WriteNumber(col.ColumnName, (char)row[col]);
                                break;
                            case TypeCode.Byte:
                                writer.WriteNumber(col.ColumnName, (byte)row[col]);
                                break;
                            case TypeCode.DBNull:
                                writer.WriteNull(col.ColumnName);
                                break;
                            case TypeCode.Empty:
                                writer.WriteString(col.ColumnName, "");
                                break;
                            case TypeCode.Object:
                                var type = row[col].GetType();
                                writer.WritePropertyName(col.ColumnName);
                                writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(row[col], type, options));
                                break;
                        }

                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }

    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
    }

    public class WeatherForecastRuntimeIgnoreConverter : JsonConverter<WeatherForecast>
    {
        public override WeatherForecast Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var wf = new WeatherForecast();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return wf;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString()!;
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Date":
                            DateTimeOffset date = reader.GetDateTimeOffset();
                            wf.Date = date;
                            break;
                        case "TemperatureCelsius":
                            int temperatureCelsius = reader.GetInt32();
                            wf.TemperatureCelsius = temperatureCelsius;
                            break;
                        case "Summary":
                            string summary = reader.GetString()!;
                            wf.Summary = string.IsNullOrWhiteSpace(summary) ? "N/A" : summary;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, WeatherForecast wf, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Date", wf.Date);
            writer.WriteNumber("TemperatureCelsius", wf.TemperatureCelsius);
            if (!string.IsNullOrWhiteSpace(wf.Summary) && wf.Summary != "N/A")
            {
                writer.WriteString("Summary", wf.Summary);
            }

            writer.WriteEndObject();
        }
    }
}
