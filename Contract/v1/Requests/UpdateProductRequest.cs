using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Contract.v1.Requests
{
    public class UpdateProductRequest
    {
        
        public string Code { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
    }
}
