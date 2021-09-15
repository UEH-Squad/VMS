﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class CreateOrgProfileViewModel
    {
        [Required(ErrorMessage = "Tên tổ chức không được để trống")]
        [MinLength(2, ErrorMessage = "Tên tổ chức tối thiểu 2 kí tự")]
        [MaxLength(50, ErrorMessage = "Tên tổ chức tối đa 50 kí tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Tầm nhìn và sứ mệnh không được để trống")]
        public string Mission { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MinLength(10, ErrorMessage = "Số điện thoại tối thiểu 10 số")]
        [MaxLength(20, ErrorMessage = "Số điện thoại tối đa 12 số")]
        public string PhoneNumber2 { get; set; }

    }
}
