using System.IO;
using System.Data;
using System.Data.OleDb;
using Jumpcity.IO;
using Jumpcity.Utility.Extend;

namespace Jumpcity.Office.Excel
{
    /// <summary>
    /// 对于Excel文档的快速操作
    /// </summary>
    public static class ExcelManager
    {
        /// <summary>    
        /// 把数据从Excel装载到DataTable    
        /// </summary>    
        /// <param name="serverPath">Excel文档的服务器路径</param>    
        /// <param name="sheetName">工作表名</param>
        /// <returns>如果文件路径有问题，或者文件不存在，则返回NULL，否则返回读取出来的DataTable</returns> 
        public static DataTable ToTable(string serverPath, string sheetName = "Sheet1")
        {
            string path = FileHelper.MapPath(serverPath);
            if (General.IsNullable(path))
                return null;

            FileInfo file = new FileInfo(path);
            if (!file.Exists)
                return null;

            if (General.IsNullable(sheetName))
                sheetName = "Sheet1";
            
            DataTable dataTable = new DataTable();

            using (OleDbConnection conn = GetConnection(file))
            { 
                OleDbDataAdapter adapter = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), conn);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        private static OleDbConnection GetConnection(FileInfo file)
        {
            string conn = string.Empty;
            string extension = file.Extension;
            if (extension.Equals(".xls"))
                conn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", file.FullName);
            else if (extension.Equals(".xlsx"))
                conn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", file.FullName);

            return new OleDbConnection(conn);
        }
    }
}
