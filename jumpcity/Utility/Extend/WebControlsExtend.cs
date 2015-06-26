using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Jumpcity.Utility.Extend
{
    /// <summary>
    /// 自定义针对服务器控件类的扩展
    /// </summary>
    public static class WebControlsExtend
    {
        /// <summary>
        /// 判断当前的RepeaterItem的ItemType是否是一个数据项
        /// </summary>
        /// <param name="item">当前要被检测的RepeaterItem</param>
        /// <returns>如果是数据项返回True，否则返回False</returns>
        public static bool IsDataItem(this RepeaterItem item)
        {
            return (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem);
        }

        /// <summary>
        /// 判断当前的DataListItem的ItemType是否是一个数据项
        /// </summary>
        /// <param name="item">当前要被检测的DataListItem</param>
        /// <returns>如果是数据项返回True，否则返回False</returns>
        public static bool IsDataItem(this DataListItem item)
        {
            return (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem);
        }

        /// <summary>
        /// 获取当前CheckBoxList的全部选中项的值
        /// </summary>
        /// <param name="source">当前的CheckBoxList对象</param>
        /// <param name="split">设置多个选中项的值之间的分隔符，默认为逗号</param>
        /// <returns>返回按指定的分隔符分割的选中项值的字符串</returns>
        public static string GetSelectedValues(this CheckBoxList source, string split = ",")
        {
            string value = string.Empty;
            if (split == null)
                split = ",";

            if (!General.IsNullable(source.Items))
            {
                foreach (ListItem item in source.Items)
                {
                    if (item.Selected)
                        value += (item.Value + split);
                }

                if (!General.IsNullable(value))
                    value = value.TrimEnd(split.ToCharArray());
            }

            return value;
        }

        /// <summary>
        /// 判断当前上传控件正在上传的文件是否是指定的文件类型格式
        /// </summary>
        /// <param name="upload">当前的上传控件对象</param>
        /// <param name="extensions">可以匹配的文件类型格式列表</param>
        /// <returns>列表中的任意一项匹配成功返回True，否则返回False</returns>
        public static bool AllowedExtensions(this FileUpload upload, params FileExtension[] extensions)
        {
            if (!upload.HasFile)
                return false;

            if (General.IsNullable(extensions))
                return true;

            return Jumpcity.IO.FileHelper.AllowedExtensions(upload.PostedFile.InputStream, extensions);
        }

        /// <summary>
        /// 判断当前上传控件正在上传的文件是否是图片类型格式
        /// </summary>
        /// <param name="upload">当前的上传控件对象</param>
        /// <returns>文件类型格式jpg,png,gif,bmp中的任意一项匹配成功返回True，否则返回False</returns>
        public static bool AllowedImages(this FileUpload upload)
        {
            return AllowedExtensions(upload, FileExtension.JPG, FileExtension.PNG, FileExtension.GIF, FileExtension.BMP);
        }
    }
}
