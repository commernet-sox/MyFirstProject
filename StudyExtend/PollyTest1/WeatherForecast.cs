using System;

namespace PollyTest1
{
    public class WeatherForecast
    {
        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// ���϶�
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// ���϶�
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// С��
        /// </summary>
        public string Summary { get; set; }
    }
}
