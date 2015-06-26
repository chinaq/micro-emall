using System;
using System.IO;
using System.Web;
using System.Text;
using Jumpcity.Utility;
using System.Collections.Generic;
using Jumpcity.Utility.Extend;

namespace Jumpcity.IO
{
    /// <summary>
    /// 用于文件流的基本操作和转换的帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 根据指定的服务端文件相对路径读取文件并创建二进制流
        /// </summary>
        /// <param name="serviceFileName">目标服务端文件相对路径</param>
        /// <param name="bufferSize">设置缓冲区大小，默认为1024字节</param>
        /// <returns>返回创建好的二进制流，如果读取失败则返回NULL</returns>
        public static Stream CreateStream(string serviceFileName, int bufferSize = 1024)
        {
            FileStream fileStream = null;
            string fileName = MapPath(serviceFileName);

            if (fileName != null && bufferSize > 0)
            {
                try
                {
                    fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                }
                catch (Exception ex)
                {
                    if (fileStream != null)
                        fileStream.Close();

                    throw ex;
                }
            }
            return fileStream;
        }

        /// <summary>
        /// 获取客户端提交的文件流
        /// </summary>
        /// <param name="request">客户端请求对象</param>
        /// <param name="index">当上传多个文件时为文件流列表项的索引，单个文件时应为零，默认为零</param>
        /// <returns>返回获取到的二进制流对象</returns>
        public static Stream GetRequestFile(HttpRequest request, int index = 0)
        {
            Stream stream = null;
            if (request != null && index >= 0)
            {
                if (request.Files.Count > 0)
                {
                    HttpPostedFile postedfile = request.Files.Get(index);
                    stream = postedfile.InputStream;
                }
                else
                {
                    string disposition = request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
                    if (!string.IsNullOrEmpty(disposition))
                    {
                        byte[] file = request.BinaryRead(request.TotalBytes);
                        stream = new MemoryStream(file, 0, file.Length);
                    }
                }
            }

            return stream;
        }

        /// <summary>
        /// 判断指定的文件流是否为指定的文件类型格式
        /// </summary>
        /// <param name="stream">要判断的文件流</param>
        /// <param name="extensions">可以匹配的文件类型格式列表</param>
        /// <returns>如果文件类型格式列表中任意一项匹配成功则返回True，否则返回False</returns>
        public static bool AllowedExtensions(Stream stream, params FileExtension[] extensions)
        {
            if (stream == null)
                return false;

            if (General.IsNullable(extensions))
                return true;

            string style = string.Empty;
            BinaryReader reader = new BinaryReader(stream);

            try
            {
                byte buffer = reader.ReadByte();
                style = buffer.ToString();
                buffer = reader.ReadByte();
                style += buffer.ToString();
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                if (reader != null)
                    reader.Close();
                throw ex;
            }

            if (!General.IsNullable(style))
            {
                int fileStyle = style.ToInt32();

                foreach (FileExtension ext in extensions)
                {
                    if (fileStyle == (int)ext)
                        return true;
                }
            }

            return false;   
        }

        /// <summary>
        /// 判断指定的文件流是否为指定的文件类型格式
        /// </summary>
        /// <param name="buffer">要判断的文件流</param>
        /// <param name="extensions">可以匹配的文件类型格式列表</param>
        /// <returns>如果文件类型格式列表中任意一项匹配成功则返回True，否则返回False</returns>
        public static bool AllowedExtensions(byte[] buffer, params FileExtension[] extensions)
        {
            if (!General.IsNullable(buffer))
            {
                MemoryStream ms = new MemoryStream(buffer);
                ms.Position = 0;

                return AllowedExtensions(ms, extensions);
            }

            return false;
        }

        /// <summary>
        /// 判断指定的文件流是否为图片类型格式
        /// </summary>
        /// <param name="stream">要判断的文件流</param>
        /// <returns>如果文件类型格式与jpg,png,gif,bmp任意一项匹配成功则返回True，否则返回False</returns>
        public static bool AllowedImages(Stream stream)
        {
            return AllowedExtensions(stream, FileExtension.JPG, FileExtension.PNG, FileExtension.GIF, FileExtension.BMP);
        }

        /// <summary>
        /// 判断指定的文件流是否为图片类型格式
        /// </summary>
        /// <param name="buffer">要判断的文件流</param>
        /// <returns>如果文件类型格式与jpg,png,gif,bmp任意一项匹配成功则返回True，否则返回False</returns>
        public static bool AllowedImages(byte[] buffer)
        {
            return AllowedExtensions(buffer, FileExtension.JPG, FileExtension.PNG, FileExtension.GIF, FileExtension.BMP);
        }

        /// <summary>
        /// 解决文件流在某些平台下会添加多余字节的问题
        /// </summary>
        /// <param name="httpStream">待解决的文件流</param>
        /// <param name="encoding">设置编码</param>
        /// <returns>返回去除多余字节后的文件流</returns>
        public static Stream OffsetStream(Stream httpStream, Encoding encoding = null)
        {
            MemoryStream ms = new MemoryStream();
            StreamReader reader = null;
            try
            {
                httpStream.CopyTo(ms);
                ms.Position = 0;

                if (encoding == null)
                    encoding = Encoding.UTF8;

                reader = new StreamReader(ms, encoding);
                long headerLength = 0L;

                //读取第一行
                string firstLine = reader.ReadLine();

                //计算偏移（字符串长度+回车换行2个字符）
                headerLength += encoding.GetBytes(firstLine).LongLength + 2;

                //一直读到空行为止
                while (true)
                {
                    var line = reader.ReadLine();

                    //若到头，则直接返回
                    if (line == null)
                        break;

                    //若未到头，则计算偏移（字符串长度+回车换行2个字符）
                    headerLength += encoding.GetBytes(line).LongLength + 2;
                    if (line == "")
                        break;
                }

                //设置偏移，以开始读取文件内容
                ms.Position = headerLength;

                //减去末尾的字符串：“\r\n--\r\n”
                ms.SetLength(ms.Length - encoding.GetBytes(firstLine).LongLength - 3 * 2);

                return ms;
            }
            catch (Exception ex)
            {
                if (ms != null)
                    ms.Dispose();

                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// 从文件的开始位置读取文件内容直到末尾
        /// </summary>
        /// <param name="serviceFileName">目标服务端文件的相对路径</param>
        /// <param name="encoding">设置编码</param>
        /// <returns>返回读取到的内容，如果读取失败则返回空字符串</returns>
        public static string ReadFileToEnd(string serviceFileName, Encoding encoding = null)
        {
            string readText = string.Empty;
            string fileName = MapPath(serviceFileName);

            if (fileName != null)
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;

                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(fileName, encoding);
                    readText = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    readText = string.Empty;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return readText;
        }

        /// <summary>
        /// 将指定字符串内容写入指定路径的文件
        /// </summary>
        /// <param name="content">要写入的文本内容</param>
        /// <param name="serviceFileName">目标文件的服务器相对路径</param>
        /// <param name="isAppend">指示是否为追加操作，为false代表覆盖操作，默认为false</param>
        /// <param name="encoding">设置编码，默认为utf-8编码</param>
        /// <param name="bufferSize">设置缓冲区大小，默认为1024字节</param>
        /// <returns>写入成功返回True，如果路径有误则返回False</returns>
        public static bool WriterContent(string content, string serviceFileName, bool isAppend = false, Encoding encoding = null, int bufferSize = 1024)
        {
            string fileName = MapPath(serviceFileName);

            if (fileName != null)
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;

                using (StreamWriter writer = new StreamWriter(fileName, isAppend, encoding, bufferSize))
                {
                    writer.Write(content);
                    writer.Flush();
                    writer.Close();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 粗略检查指定的字符串是否为一个服务端相对路径
        /// </summary>
        /// <param name="servicePath">要检查的服务段相对路径字符串</param>
        /// <param name="isFile">为True代表将字符串作为文件路径来检查，否则作为文件夹路径来检查。默认为True</param>
        /// <returns>如果检查通过返回True，否则返回False</returns>
        public static bool CheckServicePath(string servicePath, bool isFile = true)
        {
            bool flag = (
               !string.IsNullOrWhiteSpace(servicePath)
             && servicePath.IndexOf("~") == 0
             && servicePath.IndexOf("/") == 1
            );

            if (flag && isFile)
                flag = (servicePath.LastIndexOf(".") != -1);

            return flag;
        }

        /// <summary>
        /// 根据指定的服务器相对路径创建所有目录和子目录
        /// </summary>
        /// <param name="serviceDir">指定的服务器相对路径</param>
        /// <returns>创建成功返回True，如果路径不符合规范返回False</returns>
        public static bool CreateDirectory(string serviceDir)
        {
            if (serviceDir.LastIndexOf(".") != -1)
                serviceDir = serviceDir.Substring(0, serviceDir.LastIndexOf("/"));

            string dir = MapPath(serviceDir, false);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将指定的服务器相对路径转换成HTTP形式的URL
        /// </summary>
        /// <param name="serviceFileName">指定的服务器文件相对路径</param>
        /// <returns>返回转换后的URL，如果传入的相对路径格式不正确，则返回空字符串</returns>
        public static string GetHttpURL(string serviceFileName)
        {
            string httpUrl = string.Empty;

            if (CheckServicePath(serviceFileName))
            {
                Uri uri = HttpContext.Current.Request.Url;
                httpUrl = string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority, uri.IsLoopback ? "/" + uri.Segments[1].Replace("/", "") : "");
                httpUrl = serviceFileName.Replace("~", httpUrl);
            }

            return httpUrl;
        }

        /// <summary>
        /// 返回与Web服务器上的指定虚拟路径相对应的物理文件路径
        /// </summary>
        /// <param name="servicePath">指定的服务器相对路径</param>
        /// <param name="isFile">指示指定的相对路径是否为文件路径，默认为True</param>
        /// <returns>返回与servicePath相对应的物理文件路径,如果路径检查失败则返回NULL</returns>
        public static string MapPath(string servicePath, bool isFile = true)
        {
            if (CheckServicePath(servicePath, isFile))
                return HttpContext.Current.Server.MapPath(servicePath);

            return null;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="servicePath">要删除文件的服务器相对路径</param>
        /// <returns>删除成功返回True，否则返回False</returns>
        public static bool DeleteFile(string servicePath)
        {
            servicePath = MapPath(servicePath);

            if (servicePath != null)
            {
                if (File.Exists(servicePath))
                    File.Delete(servicePath);
                return !File.Exists(servicePath);
            }

            return false;
        }

        /// <summary>
        /// 删除指定目录下所有文件（不包括子目录以及子目录下的文件）
        /// </summary>
        /// <param name="serviceDir">目录的服务器相对路径</param>
        /// <returns>删除成功或者该目录下不包含任何文件则返回True，否则返回False</returns>
        public static bool ClearFiles(string serviceDir)
        {
            string dir = MapPath(serviceDir);

            if(!General.IsNullable(dir))
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                if (info.Exists)
                {
                    FileInfo[] files = info.GetFiles();
                    if (!General.IsNullable(files))
                    {
                        foreach (FileInfo file in files)
                            file.Delete();
                    }

                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 下载指定文件
        /// </summary>
        /// <param name="path">文件的物理路径(完全限定名)</param>
        /// <param name="contentType">文件的MIME类型</param>
        public static void DownLoadFile(string path, string contentType)
        {
            if (CheckServicePath(path))
            {
                string fileName = Path.GetFileName(path);

                HttpResponse response = HttpContext.Current.Response;

                response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                response.ContentType = contentType;
                response.WriteFile(path);
            }
        }
    }
}
