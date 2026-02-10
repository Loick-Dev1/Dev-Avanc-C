using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvancedDevSample.Application.DTOs
{
    public class ChangePriceRequest
    {
        /// <summary>
        /// Nouveau prix du produit
        /// Doit etre strictement positif
        /// </summary>
        /// 
        [Required]
        [Range (0.01, double.MaxValue)]

        public decimal NewPrice { get; set; }
        //"reason": "augmentation fournisseur",
        //"effectiveDate": "2026-01-01"
    }
}
