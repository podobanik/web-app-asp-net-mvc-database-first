using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppAspNetMvcDatabaseFirst.ViewModels
{
    public class ClientTypeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тип клиента", Order = 10)]
        public string Name { get; set; }
    }
}