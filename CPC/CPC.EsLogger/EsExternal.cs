namespace CPC.Logger
{
    public class EsExternal : EsBody
    {
        private string _index;

        /// <summary>
        /// 索引
        /// </summary>
        public string Index
        {
            get => _index;
            set => _index = value?.ToLowerInvariant();
        }

        private string _type;

        /// <summary>
        /// 索引类型
        /// </summary>
        public string Type
        {
            get => _type;
            set => _type = value?.ToLowerInvariant();
        }

        /// <summary>
        /// 索引后缀格式
        /// </summary>
        public string SuffixFormat { get; set; }

    }
}
