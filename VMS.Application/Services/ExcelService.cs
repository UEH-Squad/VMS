using System;
using System.IO;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using VMS.Common.Enums;
using VMS.Domain.Models;

namespace VMS.Application.Services
{
    public class ExcelService : IExcelService
    {
        private const int MaxColumn = 6;
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
                    if (sheet.Dimension.End.Column != MaxColumn)
                    {
                        return false; 
                    }

                    List<CreateAccountViewModel> accounts = new();

                    int rowCount = sheet.Dimension.End.Row;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        CreateAccountViewModel account = new();

                        account.StudentId = sheet.Cells[row, 1].Value.ToString();
                        account.FullName = sheet.Cells[row, 2].Value.ToString() + " " + sheet.Cells[row, 3].Value.ToString();
                        account.Class = sheet.Cells[row, 4].Value?.ToString();
                        account.Course = sheet.Cells[row, 5].Value.ToString();
                        account.Email = sheet.Cells[row, 6].Value.ToString();

                        accounts.Add(account);
                    }

                    await _adminService.AddListUsersAsync(accounts, role);
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
