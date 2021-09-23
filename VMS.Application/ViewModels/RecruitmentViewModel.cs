using System;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class RecruitmentViewModel
    {
        public int Id { get; set; }
        public double? Rating { get; set; }
        public User User {  get; set; }
    }
}
