using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumpcity.Office.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Jumpcity.Utility.Extend
{
    public static class ExcelExtend
    {
        /// <summary>
        /// 往当前工作簿中插入一个Sheet工作区
        /// </summary>
        /// <param name="workbookPart">当前的工作簿对象</param>
        /// <param name="sheetName">指定新插入工作区的名称</param>
        /// <returns>返回一个新的工作区节点对象WorksheetPart</returns>
        public static WorksheetPart InsertSheet(this WorkbookPart workbookPart, string sheetName = null)
        {
            //创建一个新的WorkssheetPart
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            //取得Sheet集合
            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            //得到Sheet的唯一序号
            IEnumerable<Sheet> sheetList = sheets.Elements<Sheet>();
            uint sheetId = sheetList.Count() > 0 ? sheetList.Select(s => s.SheetId.Value).Max() + 1 : 1;
            string sheetTempName = "Sheet" + sheetId;

            if (!General.IsNullable(sheetName) && sheetList.Where(s => s.Name.Equals(sheetName)).Count() <= 0)
                sheetTempName = sheetName;

            //创建Sheet实例并将它与sheets关联
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetTempName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        /// <summary>
        /// 在当前工作簿中创建一个可以在各Sheet中共用的存放字符串的容器对象
        /// </summary>
        /// <param name="workbookPart">当前的工作簿对象</param>
        /// <returns>返回创建后的共享容器对象</returns>
        public static SharedStringTablePart CreateSharedPart(this WorkbookPart workbookPart)
        {
            SharedStringTablePart shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (shareStringPart == null)
                shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
            return shareStringPart;
        }

        /// <summary>
        /// 向当前的SharedStringTablePart添加字符串
        /// </summary>
        /// <param name="shareStringPart">sharedStringTablePart内容</param>
        /// <param name="text">字符串内容</param>
        /// <returns>返回该字符串内容在共享区中的索引</returns>
        public static int InsertItem(this SharedStringTablePart shareStringPart, string text)
        {
            //检测SharedStringTable是否存在，如果不存在，则创建一个
            if (shareStringPart.SharedStringTable == null)
                shareStringPart.SharedStringTable = new SharedStringTable();

            List<SharedStringItem> items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToList<SharedStringItem>();

            //查看目标字符串是否存在
            int index = items.FindIndex(sst => sst.InnerText.Equals(text));

            if (index == -1)
            {
                //如果目标字符串不存在，则创建一个，同时把SharedStringTable的最后一个Elements的"索引+1"返回
                shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
                shareStringPart.SharedStringTable.Save();
                index = 0;
            }

            return index;
        }

        /// <summary>
        /// 向当前工作区的表中批量插入单元格
        /// </summary>
        /// <param name="worksheetPart">当前的工作区对象</param>
        /// <param name="cells">设置所需要插入的单元格集合</param>
        /// <returns>返回成功插入的单元格数量</returns>
        public static int InsertCell(this WorksheetPart worksheetPart, List<ExcelCell> cells)
        {
            int flag = 0;

            if (!General.IsNullable(cells))
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> sheetRows = sheetData.Elements<Row>();

                foreach (ExcelCell cell in cells)
                {
                    uint rowIndex = cell.RowIndex;
                    string cellReference = cell.ReferenceName;
                    IEnumerable<Cell> cList = null;
                    Row row = sheetRows.Where(r => r.RowIndex == rowIndex).FirstOrDefault();
                    //如果指定的行存在，则直接返回该行，否则插入新行 
                    if (row == null)
                    {
                        row = new Row { RowIndex = rowIndex };
                        sheetData.Append(row);
                    }
                    
                    cList = row.Elements<Cell>();
                    Cell newCell = cList.Where(c => c.CellReference.Value == cellReference).FirstOrDefault();

                    //如果该行没有指定ColumnName的列，则插入新列，否则直接返回该列
                    if (newCell == null)
                    {
                        newCell = new Cell() { CellReference = cellReference };

                        //列必须按(字母)顺序插入，因此要先根据"列引用字符串"查找插入的位置
                        Cell refCell = null;
                        foreach (Cell c in cList)
                        {
                            if (string.Compare(c.CellReference.Value, cellReference, true) > 0)
                            {
                                refCell = c;
                                break;
                            }
                        }

                        row.InsertBefore(newCell, refCell);
                    }
                    newCell.DataType = new EnumValue<CellValues>(cell.Type);
                    newCell.CellValue = new CellValue(cell.Value);
                    worksheetPart.Worksheet.Save();
                    flag++;
                }
            }

            return flag;
        }

        /// <summary>
        /// 向当前工作区的表中插入一个单元格
        /// </summary>
        /// <param name="worksheetPart">当前的工作区对象</param>
        /// <param name="cell">设置要插入的单元格信息</param>
        /// <returns>返回插入成功的数量</returns>
        public static bool InsertCell(this WorksheetPart worksheetPart, ExcelCell cell)
        {
            return InsertCell(worksheetPart, new List<ExcelCell> { cell }) > 0;
        }
    }
}
