using OfficeOpenXml;
using System;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.GenericRepository;

namespace VMS.Application.Services
{
    public class ExportExcelService :  IExportExcelService
    {
        public byte[] ResultExportToExcel(PaginatedList<ListVolunteerViewModel> list, int actId)
        {
            var stream = new System.IO.MemoryStream();
            using var xlPackage = new ExcelPackage(stream);
            var worksheet = xlPackage.Workbook.Worksheets.Add("sheet1");
            worksheet.Cells["A1"].Value = "STT";
            worksheet.Cells["B1"].Value = "HỌ VÀ TÊN";
            worksheet.Cells["C1"].Value = "MSSV";
            worksheet.Cells["D1"].Value = "LỚP";
            worksheet.Cells["E1"].Value = "KHOÁ";
            worksheet.Cells["F1"].Value = "KHOA";
            worksheet.Cells["G1"].Value = "EMAIL";
            worksheet.Cells["H1"].Value = "MONG MUỐN";
            worksheet.Cells["I1"].Value = "CAM KẾT";

            int row = 2;
            foreach (var item in list.Items)
            {
                worksheet.Cells[row, 1].Value = row - 1;
                worksheet.Cells[row, 2].Value = item.User.FullName;
                worksheet.Cells[row, 3].Value = item.User.StudentId;
                worksheet.Cells[row, 4].Value = item.User.Class;
                worksheet.Cells[row, 5].Value = item.User.Course;
                worksheet.Cells[row, 6].Value = item.User.FacultyId == null? "": item.User.Faculty.Name;
                worksheet.Cells[row, 7].Value = item.User.Email;
                worksheet.Cells[row, 8].Value = item.Desire;
                worksheet.Cells[row, 9].Value = item.IsCommit ? "" : "X";

                row++;
            }
            worksheet.Cells[worksheet.Dimension.Address].Style.Font.Size = 13;
            worksheet.Cells[worksheet.Dimension.Address].Style.Font.Name = "Times New Roman";
            worksheet.Cells["A1:I1"].Style.Font.Bold = true;
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(10,50);
            xlPackage.Workbook.Properties.Title = $@"DSTNV_{actId}_{DateTime.Now.ToFileTime()}";
            xlPackage.Save();
            return xlPackage.GetAsByteArray();
        } 
    }
}
