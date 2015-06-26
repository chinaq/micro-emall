using System;
using System.Collections.Generic;

namespace Jumpcity.Utility
{
    /// <summary>
    /// 该类用于枚举所有的HTTP提交类型，无法继承该类。
    /// </summary>
    [Serializable]
    public sealed class HttpMethod
    {
        /// <summary>
        /// 表示提交类型为GET
        /// </summary>
        public const string GET = "GET";

        /// <summary>
        /// 表示提交类型为POST
        /// </summary>
        public const string POST = "POST";

        /// <summary>
        /// 表示提交类型为PUT
        /// </summary>
        public const string PUT = "PUT";

        /// <summary>
        /// 表示提交类型为DELETE
        /// </summary>
        public const string DELETE = "DELETE";
    }

    /// <summary>
    /// 该类用于检索所有的MIME类型。
    /// </summary>
    [Serializable]
    public static class MimeType
    {
        /// <summary>
        /// Android应用的安装包类型
        /// </summary>
        public const string APK = "application/vnd.android.package-archive";

        /// <summary>
        /// HTTP编码类型，多用作POST提交时设置Content-Type
        /// </summary>
        public const string UrlEncoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// 未知类型
        /// </summary>
        public const string Octet = "application/octet-stream";

        /// <summary>
        /// 可移植文档格式 (PDF)
        /// </summary>
        public const string PDF = "application/pdf";
        
        /// <summary>
        /// RTF 格式 (RTF)
        /// </summary>
        public const string RTF = "application/rtf";

        /// <summary>
        /// SOAP 文档
        /// </summary>
        public const string SOAP = "application/soap+xml";
        
        /// <summary>
        /// 压缩格式
        /// </summary>
        public const string ZIP = "application/zip";

        /// <summary>
        /// 图形交换格式 (GIF)
        /// </summary>
        public const string GIF = "image/gif";
        
        /// <summary>
        /// 图像压缩格式(PNG)
        /// </summary>
        public const string PNG = "image/png";
        
        /// <summary>
        /// 联合专家组 (JPEG) 格式
        /// </summary>
        public const string JPEG = "image/jpeg";

        /// <summary>
        /// 图像文件格式 (TIFF)
        /// </summary>
        public const string TIFF = "image/tiff";

        /// <summary>
        /// HTML格式
        /// </summary>
        public const string HTML = "text/html";

        /// <summary>
        /// 纯文本格式
        /// </summary>
        public const string Plain = "text/plain";

        /// <summary>
        /// 文本RTF格式
        /// </summary>
        public const string RichText = "text/richtext";

        /// <summary>
        /// XML格式
        /// </summary>
        public const string XML = "text/xml";

        /// <summary>
        /// JSON格式
        /// </summary>
        public const string JSON = "application/json";

        /// <summary>
        /// Excel文档格式
        /// </summary>
        public const string Excel = "application/ms-excel";

        /// <summary>
        /// 获取MimeType类里所有被记录的Mime类型集合
        /// </summary>
        public static Dictionary<string, string> MimeTypes
        {
            get
            {
                Type rootType = typeof(MimeType);
                Dictionary<string, string> types = new Dictionary<string, string>();
                foreach (var type in rootType.GetFields())
                {
                    if (type.IsLiteral)
                        types.Add(type.Name, type.GetValue(null).ToString());
                }
                return types;
            }
        }
    }

    /// <summary>
    /// 表示文件真实类型的枚举
    /// </summary>
    [Serializable]
    public enum FileExtension
    {
        /// <summary>
        /// 文件为JPG类型
        /// </summary>
        JPG = 255216,

        /// <summary>
        /// 文件为GIF类型
        /// </summary>
        GIF = 7173,

        /// <summary>
        /// 文件为BMP类型
        /// </summary>
        BMP = 6677,

        /// <summary>
        /// 文件为PNG类型
        /// </summary>
        PNG = 13780,

        /// <summary>
        /// 文件为EXE或者DLL类型
        /// </summary>
        EXEAndDLL = 7790,

        /// <summary>
        /// 文件为RAR类型
        /// </summary>
        RAR = 8297,

        /// <summary>
        /// 文件为ZIP类型
        /// </summary>
        ZIP = 8075,

        /// <summary>
        /// 文件为XML类型
        /// </summary>
        XML = 6063,

        /// <summary>
        /// 文件为HTML类型
        /// </summary>
        HTML = 6033,

        /// <summary>
        /// 文件为ASPX类型
        /// </summary>
        ASPX = 239187,

        /// <summary>
        /// 文件为TXT类型
        /// </summary>
        TXT = 102100,

        /// <summary>
        /// 文件为SQL脚本类型
        /// </summary>
        SQL = 255254,

        /// <summary>
        /// 文件为FLASH的SWF类型
        /// </summary>
        SWF = 6787,

        /// <summary>
        /// 文件为JAVASCRIPT脚本类型
        /// </summary>
        JAVASCRIPT = 119105,

        /// <summary>
        /// 文件为CSharp脚本类型
        /// </summary>
        CSHARP = 117115,

        /// <summary>
        /// 文件为7Z压缩包类型
        /// </summary>
        SEVENZ = 55122
    }
}