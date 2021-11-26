using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebAppAspNetMvcDatabaseFirst.ViewModels
{
    public class OrderViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        [Required]
        [Display(Name = "Услуга", Order = 10)]
        public string Procedure { get; set; }

        /// <summary>
        /// Описание услуги
        /// </summary>

        [Display(Name = "Описание услуги", Order = 20)]
        public string Description { get; set; }
        
    }
}