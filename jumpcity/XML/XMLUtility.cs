using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Jumpcity.IO;
using Jumpcity.Utility.Extend;

namespace Jumpcity.XML
{
    /// <summary>
    /// 用于处理XML文件的工具类
    /// </summary>
	public static class XMLUtility
	{
        /// <summary>
        /// 将指定的实体对象序列化为XML文档
        /// </summary>
        /// <typeparam name="T">实体对象的类型</typeparam>
        /// <param name="entity">要序列化的实体对象</param>
        /// <param name="isOmit">指示序列化时是否需要编写XML声明</param>
        /// <param name="fileName">如果要将序列化后的内容保存到文件，该参数用于指定保存文件的服务器绝对路径(以~开头的路径)</param>
        /// <param name="rootName">指定序列化时XML文档根节点的名称，默认使用对象的类名</param>
        /// <returns>如果保存为文件，则返回保存后的文件所在路径，否则返回XML文档内容</returns>
        public static string Serialize<T>(T entity, bool isOmit = false, string fileName = "", string rootName = "")
        {
            Stream stream = null;
            XmlWriter writer = null;
            string result = string.Empty;
            XmlSerializer serializer = null;
            Type type = typeof(T);

            if (!General.IsNullable(rootName))
            {
                XmlRootAttribute rootAttr = new XmlRootAttribute(rootName);
                serializer = new XmlSerializer(type, rootAttr);
            }
            else
                serializer = new XmlSerializer(type);

            try
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    string path = FileHelper.MapPath(fileName);
                    if (!File.Exists(path))
                    {
                        if (!FileHelper.CreateDirectory(fileName))
                            throw new Exception(string.Format("无法为指定的路径：{0}创建相应的目录结构", fileName));
                    }

                    stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    result = fileName;
                }
                else
                    stream = new MemoryStream();

                writer = XmlWriter.Create(
                    stream,
                    new XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        Indent = true,
                        OmitXmlDeclaration = isOmit
                    }
                );

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(writer, entity, namespaces);

                if (stream is MemoryStream)
                    result = Encoding.UTF8.GetString(((MemoryStream)stream).ToArray());
            }
            catch (Exception ex)
            {
                result = string.Empty;
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();

                if (stream != null)
                    stream.Close();
            }

            return result;
        }

        /// <summary>
        /// 将指定的XML文档内容或者文件反序列化为指定的实体对象
        /// </summary>
        /// <typeparam name="T">指定实体对象的类型</typeparam>
        /// <param name="xml">XML文档的内容或者文件地址</param>
        /// <param name="isFilePath">指示xml参数的值是否是一个文件路径</param>
        /// <param name="rootName">指定序列化时使用的XMl文档根节点名称，如果序列化时未指定过，此处也为默认即可</param>
        /// <returns>返回反序列化后的实体对象实例</returns>
        public static T DeSerialize<T>(string xml, bool isFilePath = false, string rootName = "")
        {
            T entity = default(T);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                Type type = typeof(T);
                XmlSerializer serializer = null;

                if (!General.IsNullable(rootName))
                {
                    XmlRootAttribute rootAttr = new XmlRootAttribute(rootName);
                    serializer = new XmlSerializer(type, rootAttr);
                }
                else
                    serializer = new XmlSerializer(type);

                if (isFilePath)
                    xml = FileHelper.ReadFileToEnd(xml);

                if (!string.IsNullOrWhiteSpace(xml))
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                    {
                        Object obj = serializer.Deserialize(reader);
                        entity = (T)obj;
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// 将指定的XML文件反序列化为指定的实体对象
        /// </summary>
        /// <typeparam name="T">指定实体对象的类型</typeparam>
        /// <param name="fileName">要反序列化的XML文件路径</param>
        /// <param name="rootName">指定序列化时使用的XMl文档根节点名称，如果序列化时未指定过，此处也为默认即可</param>
        /// <returns>返回反序列化后的实体对象实例</returns>
        public static T DeSerializeFile<T>(string fileName, string rootName = "")
        {
            return DeSerialize<T>(fileName, true, rootName);
        }
    }
}
