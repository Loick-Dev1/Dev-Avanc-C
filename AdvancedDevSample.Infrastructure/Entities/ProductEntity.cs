using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Infrastructure.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
