using System;
using System.IO;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using VMS.Common.Enums;
using VMS.GenericRepository;
using System.Linq;

namespace VMS.Application.Services
{
    public class ExcelService : IExcelService
    {
        private const int MaxInputUserColumn = 6;
        private const int MaxInputOrgColumn = 5;
        private const long MaxFileSize = 1024 * 1024 * 5;

        private IAdminService _adminService;

        public ExcelService(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<bool> AddListAccountsFromExcelFileAsync(IBrowserFile file, Role role)
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
                    if (sheet.Dimension.End.Column != (role == Role.User ? MaxInputUserColumn : MaxInputOrgColumn))
                    {
                        return false;
                    }

                    List<CreateAccountViewModel> accounts = new();

                    int rowCount = sheet.Dimension.End.Row;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        CreateAccountViewModel account = GetAccountByRole(sheet, row, role);

                        accounts.Add(account);
                    }

                    await _adminService.AddListAccountsAsync(accounts, role);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static CreateAccountViewModel GetAccountByRole(ExcelWorksheet sheet, int row, Role role)
        {
            if (role == Role.User)
            {
                return new CreateAccountViewModel()
                {
                    StudentId = sheet.Cells[row, 1].Value.ToString(),
                    FullName = sheet.Cells[row, 2].Value.ToString() + " " + sheet.Cells[row, 3].Value.ToString(),
                    Class = sheet.Cells[row, 4].Value?.ToString(),
                    Course = sheet.Cells[row, 5].Value.ToString(),
                    Email = sheet.Cells[row, 6].Value.ToString()
                };
            }
            else
            {
                return new CreateAccountViewModel()
                {
                    Email = sheet.Cells[row, 2].Value.ToString(),
                    FullName = sheet.Cells[row, 3].Value.ToString(),
                    Course = sheet.Cells[row, 4].Value.ToString(),
                    Password = sheet.Cells[row, 5].Value.ToString()
                };
            }
        }

        public byte[] ExportListAccountToExcel(List<AccountViewModel> accounts, Role role)
        {
            using Stream stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage excelPackage = new(stream);

            IEnumerable<string> groups = accounts.GroupBy(x => x.Course)
                                                 .OrderByDescending(x => x.Key)
                                                 .Select(x => x.Key);

            foreach (var group in groups)
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(group);
                SetTemplateSheetByRole(sheet, role);

                int MaxOutputColumn = sheet.Dimension.End.Column;

                var accountsInGroup = accounts.Where(x => x.Course == group);

                foreach (var account in accountsInGroup)
                {
                    SetRowValueByRole(sheet, sheet.Dimension.End.Row + 1, account, role);
                }

                sheet.Cells[sheet.Dimension.Address].Style.Font.Size = 13;
                sheet.Cells[sheet.Dimension.Address].Style.Font.Name = "Times New Roman";
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns(10, 50);
            }

            excelPackage.Workbook.Properties.Title = $@"DSTK_{role}_{DateTime.Now:dd-MM-yyyy}";
            excelPackage.Save();

            return excelPackage.GetAsByteArray();
        }

        private static void SetTemplateSheetByRole(ExcelWorksheet sheet, Role role)
        {
            switch (role)
            {
                case Role.User:
                    sheet.Cells["A1"].Value = "STT";
                    sheet.Cells["B1"].Value = "EMAIL";
                    sheet.Cells["C1"].Value = "HỌ VÀ TÊN";
                    sheet.Cells["D1"].Value = "LỚP";
                    sheet.Cells["E1"].Value = "KHOÁ";
                    sheet.Cells["F1"].Value = "KHOA";
                    sheet.Cells["A1:F1"].Style.Font.Bold = true;
                    break;

                case Role.Organization:
                    sheet.Cells["A1"].Value = "STT";
                    sheet.Cells["B1"].Value = "EMAIL";
                    sheet.Cells["C1"].Value = "TÊN TỔ CHỨC";
                    sheet.Cells["D1"].Value = "CẤP";
                    sheet.Cells["A1:D1"].Style.Font.Bold = true;
                    break;
            }
        }

        private static void SetRowValueByRole(ExcelWorksheet sheet, int row, AccountViewModel account, Role role)
        {
            switch (role)
            {
                case Role.User:
                    sheet.Cells[row, 1].Value = row - 1;
                    sheet.Cells[row, 2].Value = account.Email;
                    sheet.Cells[row, 3].Value = account.FullName;
                    sheet.Cells[row, 4].Value = account.Class;
                    sheet.Cells[row, 5].Value = account.Course;
                    sheet.Cells[row, 6].Value = account.Faculty;
                    break;

                case Role.Organization:
                    sheet.Cells[row, 1].Value = row - 1;
                    sheet.Cells[row, 2].Value = account.Email;
                    sheet.Cells[row, 3].Value = account.FullName;
                    sheet.Cells[row, 4].Value = account.Class;
                    break;
            }
        }
    }
}
