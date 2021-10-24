using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class ExportExcelService : IExportExcelService
    {
        public Task ResultExportToExcel(PaginatedList<ListVolunteerViewModel> list)
        {
            throw new NotImplementedException();
        }
    }
}
