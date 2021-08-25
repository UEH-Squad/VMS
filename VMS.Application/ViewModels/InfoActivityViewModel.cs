using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class InfoActivityViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public DateTime ActivityPostDate { get; set; }
        public DateTime ActivityStartDate { get; set; }
        public DateTime ActivityEndDate { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityMission { get; set; }
        public string ActivityCommission { get; set; }
        public string ActivityRequirement { get; set; }
        public string ActivityAddress { get; set; }
        public List<Skill> ActivitySkill { get; set; }
        public string ActivityOrgId{ get; set; }
    }
}