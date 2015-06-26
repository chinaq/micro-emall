using System;
using Jumpcity.IO;
using System.Reflection;
using System.Collections.Generic;
using Jumpcity.Utility.Extend;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Jumpcity.Office.Excel
{
    public class ExcelDocument
    {
        private string _excelFileName;
        private List<ExcelCell> _cells = null;

        public string FileName
        {
            get { return _excelFileName; }
            set { _excelFileName = value; }
        }

        public string AbsoluteFileName
        {
            get 
            {
                if (!General.IsNullable(_excelFileName))
                    return FileHelper.MapPath(_excelFileName);
                return null;
            }
        }

        public string HttpFileName
        {
            get
            {
                if (!General.IsNullable(_excelFileName))
                    return FileHelper.GetHttpURL(_excelFileName);
                return null;
            }
        }

        public List<ExcelCell> Cells
        {
            get 
            {
                if (_cells == null)
                    _cells = new List<ExcelCell>();
                return _cells;
            }
            set { _cells = value; }
        }

        public ExcelDocument(string excelFileName, List<ExcelCell> cells)
        {
            this._excelFileName = excelFileName;
            this._cells = cells;
        }
        public ExcelDocument() : this(null, null) { }

        public bool CreatExcel(string sheetName)
        {
            bool flag = false;
            string fileName = this.AbsoluteFileName;

            if (!General.IsNullable(fileName))
            {
                using (SpreadsheetDocument excelDocument = CreateDocument(fileName))
                {
                    //添加工作表
                    WorksheetPart worksheetPart = excelDocument.WorkbookPart.InsertSheet(sheetName);
                    flag = worksheetPart.InsertCell(this._cells) > 0;
                }
            }

            return flag;
        }

        public bool CreateExcel<T>(T entity, string sheetName) where T : class
        {
            this.Cells = ExcelCell.ToList(entity);
            return CreatExcel(sheetName);
        }

        /// <summary>
        /// 创建一个SpreadsheetDocument对象
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <returns></returns>
        private SpreadsheetDocument CreateDocument(string excelFileName)
        {
            SpreadsheetDocument excel = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook, true);
            WorkbookPart workbookpart = excel.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            return excel;
        }    
    }
}
