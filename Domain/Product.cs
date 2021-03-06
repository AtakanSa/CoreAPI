﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Domain
{
    public class Product

    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
    }
}
