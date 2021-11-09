using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.Collections.Generic;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace VMS.Application.Services
{
    public class ExcelService : IExcelService
    {
        private const long MaxFileSize = 1024 * 1024 * 5;
        private IIdentityService _identityService;

        public ExcelService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<CreateAccountViewModel>> GetListAccountFromExcelFileAsync(IBrowserFile file)
        {
            try
            {
                using Stream stream = file.OpenReadStream(MaxFileSize);

                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using ExcelPackage excelPackage = new(memoryStream);

                List<CreateAccountViewModel> accounts = new();

                foreach (var sheet in excelPackage.Workbook.Worksheets)
                {
                    int colCount = sheet.Dimension.End.Column;
                    int rowCount = sheet.Dimension.End.Row;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        CreateAccountViewModel account = new();

                        account.StudentId = sheet.Cells[row, 1].Value.ToString();
                        account.LastName = sheet.Cells[row, 2].Value.ToString();
                        account.FirstName = sheet.Cells[row, 3].Value.ToString();
                        account.Class = sheet.Cells[row, 4].Value?.ToString();
                        account.Course = sheet.Cells[row, 5].Value.ToString();
                        account.Email = sheet.Cells[row, 6].Value.ToString();

                        accounts.Add(account);
                    }
                }

                return accounts;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
