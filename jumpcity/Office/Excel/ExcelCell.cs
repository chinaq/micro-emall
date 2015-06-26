using System;
using System.Collections;
using System.Reflection;
using Jumpcity.Utility.Extend;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Jumpcity.Office.Excel
{
    /// <summary>
    /// 设置Excel文档的单元格
    /// </summary>
    public class ExcelCell
    {
        private int _shareIndex = -1;
        private string _value = null;

        /// <summary>
        /// 获取或设置单元格的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置单元格所属的行索引号(从1开始)
        /// </summary>
        public uint RowIndex { get; set; }

        /// <summary>
        /// 获取单元格的引用字符串(例如'A1','B2'等)
        /// </summary>
        public string ReferenceName 
        {
            get { return this.Name.ToUpper() + this.RowIndex; }
        }

        /// <summary>
        /// 获取或设置单元格的值类型
        /// </summary>
        public CellValues Type { get; set; }

        /// <summary>
        /// 获取或设置单元格的值
        /// </summary>
        public string Value 
        {
            get 
            {
                if (this.Type == CellValues.SharedString && _shareIndex >= 0)
                    _value = _shareIndex.ToString();
                return _value;
            }
            set { _value = value; } 
        }

        /// <summary>
        /// 获取或设置单元格值所在的共享区的索引
        /// </summary>
        public int ShareIndex 
        {
            get { return _shareIndex; }
            set { _shareIndex = value; } 
        }

        public static List<ExcelCell> ToList<T>(T entity, bool isHeader = true) where T : class
        {
            PropertyInfo[] infoList = General.GetPropertyInfo(typeof(T));
            List<ExcelCell> cells = new List<ExcelCell>();
            ExcelCell cell = null;
            int ascNumber = 65;
            IList entityList = (entity is IList ? entity as IList : new List<T> { entity });
            uint rowIndex = 1;

            if (isHeader)
            {
                rowIndex = 2;
                foreach (PropertyInfo info in infoList)
                {
                    TypeCode tc = System.Type.GetTypeCode(info.PropertyType);
                    cell = new ExcelCell();
                    cell.Name = ascNumber.ConvertFromASCII();
                    cell.RowIndex = 1;
                    cell.Type = CellValues.String;
                    cell.Value = info.Name;
                    cells.Add(cell);
                    ascNumber++;    
                }
            }

            for (int i = 0; i < entityList.Count; i++)
            {
                var en = entityList[i];
                ascNumber = 65;

                foreach (PropertyInfo info in infoList)
                {
                    TypeCode tc = System.Type.GetTypeCode(info.PropertyType);
                    object infoValue = info.GetValue(en, null);
                    infoValue = (infoValue == null ? string.Empty : infoValue);
                    cell = new ExcelCell();
                    cell.Name = ascNumber.ConvertFromASCII();
                    cell.RowIndex = rowIndex;
                    cell.Type = CellValues.String;
                    cell.Value = (tc == TypeCode.DateTime ? ((DateTime)infoValue).ToString("yyyy-MM-dd HH:mm") : infoValue.ToString());
                    cells.Add(cell);
                    ascNumber++;
                }

                rowIndex++;
            }

            return cells;
        }
    }
}
