using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CPC
{
    /// <summary>
    /// UTF8编码；缓存类型-加快“第二次”转换速度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class XmlParser<T>
    {
        private static XmlSerializer fXmlSerializer = null;
        private static readonly UTF8Encoding Utf8 = new UTF8Encoding();

        /// <summary>
        /// 获取整个Xml文件；带头信息；无命名空间
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ToXml(T item)
        {
            if (fXmlSerializer == null)
            {
                fXmlSerializer = new XmlSerializer(typeof(T));
            }

            using (var ms = new MemoryStream())
            {
                //XmlSerializer不給Encoding，其XML宣告會是UTF-16 ；而且不能直接用Encoding.UTF8
                var xmlWriter = new XmlTextWriter(ms, Utf8);

                var xsnp = new XmlSerializerNamespaces();
                xsnp.Add(string.Empty, string.Empty);

                fXmlSerializer.Serialize(xmlWriter, item, xsnp);
                var rst = Encoding.UTF8.GetString(ms.ToArray());

                return rst;
            }
        }

        public static T FromXml(string str)
        {
            if (fXmlSerializer == null)
            {
                fXmlSerializer = new XmlSerializer(typeof(T));
            }

            using (var reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)fXmlSerializer.Deserialize(reader);
            }
        }
    }
}
