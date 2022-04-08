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
            var dt=new DataTable();
            var list = new List<string>();
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
                                dt.Columns.Add(new DataColumn(propertyName,typeof(Int32)));
                            }

                            list.Add(reader.GetInt32().ToString());
                            break;
                        case JsonTokenType.String:
                            if (!dt.Columns.Contains(propertyName))
                            {
                                dt.Columns.Add(new DataColumn(propertyName, typeof(string)));
                            }

                            list.Add(reader.GetString());
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
                                writer.WriteBoolean(col.ColumnName,(bool)row[col]);
                                break;
                        case TypeCode.Int32:
                                writer.WriteNumber(col.ColumnName,(int)row[col]);
                                break;
                        case TypeCode.DateTime:
                                writer.WriteString(col.ColumnName,row[col].ToString());
                                break;

                    }

                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
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
