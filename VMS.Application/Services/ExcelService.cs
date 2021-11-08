using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class ExcelService : BaseService, IExcelService
    {
        private const long MaxFileSize = 1024 * 1024 * 5;

        public ExcelService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
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

                var worksheets = excelPackage.Workbook.Worksheets;

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
