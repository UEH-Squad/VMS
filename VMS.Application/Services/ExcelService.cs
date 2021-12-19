using System;
using System.IO;
using OfficeOpenXml;
using VMS.Common.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

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
    }
}
