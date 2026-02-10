using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    public class UpdateProductRequest
    {
        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        public bool? IsActive { get; set; }
    }
}