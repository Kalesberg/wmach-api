using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using API.Models;
using System.Net.Http;
using OfficeOpenXml;
using System.Data;
using System.Drawing;

namespace API.Utilities
{
    public static class ExcelGeneration
    {
        /// <summary>
        /// Get a generated excel sheet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="WorksheetName">Name of the worksheet.</param>
        /// <param name="Header">Is there a header?.</param>
        /// <param name="Format">Do we format the cells to look better?</param>
        /// <param name="ColumnFormats">A Dictionary of (index, format) values for the columns. For instance (1, "yyyy-mm-dd hh:mm") </param>
        /// <returns></returns>
        public static MemoryStream GetExcelSheet<T>(IEnumerable<T> collection, string WorksheetName, bool Header = true, bool Format = true, Dictionary<int, string> ColumnFormats = null)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(WorksheetName);
                worksheet.Cells["A1"].LoadFromCollection(collection, true);
                int totalRows = worksheet.Dimension.End.Row;
                int totalCols = worksheet.Dimension.End.Column;
                if (ColumnFormats != null)
                    ApplyColumnFormat(worksheet,ColumnFormats);
                if (Format)
                    FormatCells(worksheet, totalRows, totalCols);   
                if (Header)
                    CreateHeader(worksheet, totalRows, totalCols);   
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        /// <summary>
        /// Gets the excel sheet.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="WorksheetName">Name of the worksheet.</param>
        /// <param name="Header">if set to <c>true</c> [header].</param>
        /// <param name="Format">if set to <c>true</c> [pretty].</param>
        /// <param name="ColumnFormats">The column formats.</param>
        /// <returns></returns>
        public static MemoryStream GetExcelSheet(DataTable collection, string WorksheetName, bool Header = true, bool Format = true, Dictionary<int, string> ColumnFormats = null)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(WorksheetName);
                worksheet.Cells["A1"].LoadFromDataTable(collection, true);
                int totalRows = worksheet.Dimension.End.Row;
                int totalCols = worksheet.Dimension.End.Column;
                if (ColumnFormats != null)
                    ApplyColumnFormat(worksheet, ColumnFormats);
                if (Format)
                    FormatCells(worksheet, totalRows, totalCols);
                if (Header)
                    CreateHeader(worksheet, totalRows, totalCols);   
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        /// <summary>
        /// Gets the excel sheet.
        /// </summary>
        /// <param name="collections"></param>
        /// <param name="WorksheetNames"></param>
        /// <param name="Header">if set to <c>true</c> [header].</param>
        /// <param name="Format">if set to <c>true</c> [pretty].</param>
        /// <param name="ColumnFormats">The column formats.</param>
        /// <returns></returns>
        public static MemoryStream GetExcelWorkbook(Dictionary<string, DataTable> collections, string[] WorksheetNames, bool Header = true, bool Format = true, Dictionary<string, Dictionary<int, string>> ColumnFormats = null)
        {
            using (var package = new ExcelPackage())
            {
                foreach (string WorksheetName in WorksheetNames)
                {
                    var worksheet = package.Workbook.Worksheets.Add(WorksheetName);
                    worksheet.Cells["A1"].LoadFromDataTable(collections[WorksheetName], true);
                    int totalRows = worksheet.Dimension.End.Row;
                    int totalCols = worksheet.Dimension.End.Column;
                    if (ColumnFormats.ContainsKey(WorksheetName) && ColumnFormats[WorksheetName] != null)
                        ApplyColumnFormat(worksheet, ColumnFormats[WorksheetName]);
                    if (Format)
                        FormatCells(worksheet, totalRows, totalCols);
                    if (Header)
                        CreateHeader(worksheet, totalRows, totalCols);
                }
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        private static void CreateHeader(ExcelWorksheet worksheet, int totalRows, int totalCols)
        {
            var headerCells = worksheet.Cells[1, 1, 1, totalCols];
            headerCells.AutoFilter = true;
            var headerFont = headerCells.Style.Font;
            headerFont.Bold = true;
            headerFont.UnderLine = true;
            headerFont.Color.SetColor(Color.White);
            var headerFill = headerCells.Style.Fill;
            headerFill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerFill.BackgroundColor.SetColor(Color.RoyalBlue);
        }

        private static void FormatCells(ExcelWorksheet worksheet, int totalRows, int totalCols)
        {
            var allCells = worksheet.Cells[1, 1, totalRows, totalCols];
            allCells.AutoFitColumns();
            allCells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            var border = allCells.Style.Border;
            border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
              
        }

        private static void ApplyColumnFormat(ExcelWorksheet worksheet, Dictionary<int,string> Formats)
        {
            foreach (KeyValuePair<int, string> IxFormat in Formats)
            {
                worksheet.Column(IxFormat.Key).Style.Numberformat.Format = IxFormat.Value;
                worksheet.Column(IxFormat.Key).AutoFit();
            }
        }

    }

}