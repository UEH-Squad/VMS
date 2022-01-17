using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Application.Services
{
    public class ExportExcelService :  IExportExcelService
    {
        private const int MaxInputVolunteerColumn = 9;
        private const long MaxFileSize = 1024 * 1024 * 5;
        private IRecruitmentService _recruitmentService;

        public ExportExcelService(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }
        public byte[] ResultExportToExcel(List<ListVolunteerViewModel> list, int actId)
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
            foreach (var item in list)
            {
                worksheet.Cells[row, 1].Value = row - 1;
                worksheet.Cells[row, 2].Value = item.User.FullName;
                worksheet.Cells[row, 3].Value = item.User.StudentId;
                worksheet.Cells[row, 4].Value = item.User.Class;
                worksheet.Cells[row, 5].Value = item.User.Course;
                worksheet.Cells[row, 6].Value = item.User.FacultyId == null? "": item.User.Faculty.Name;
                worksheet.Cells[row, 7].Value = item.User.NotifiedEmail;
                worksheet.Cells[row, 8].Value = item.Desire;
                worksheet.Cells[row, 9].Value = item.IsCommit ? "X" : "";

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
        public async Task<bool> UpdateListVolunteerFromExcelFileAsync(IBrowserFile file, int actId )
        {
            try
            {
                using Stream stream = file.OpenReadStream(MaxFileSize);

                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using ExcelPackage excelPackage = new(memoryStream);

                foreach (var sheet in excelPackage.Workbook.Worksheets)
                {
                    if (sheet.Dimension.End.Column != MaxInputVolunteerColumn)
                    {
                        return false;
                    }

                    int rowCount = sheet.Dimension.End.Row;
                    List<string> volunteers = new();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if(sheet.Cells[row, 3].Value != null)
                        {
                            volunteers.Add(sheet.Cells[row, 3].Value.ToString());
                        }
                    }
                    await _recruitmentService.UpdateRecruitmentAsync(volunteers, actId);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
    }
}
