using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class ProductSpecification: BaseSpecification<Product>
    {
        public ProductSpecification(string? brand, string? type, string? sort): base(u => 
        (string.IsNullOrWhiteSpace(brand) || u.Brand == brand) && 
        (string.IsNullOrWhiteSpace(type) || u.Type == type)
        )
        {
            switch(sort)
            {
                case "priceAsc":
                    AddOrderBy(u => u.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(u => u.Price);
                    break;
                default:
                    AddOrderBy(u => u.Name);
                    break;
            }
        }
    }
}
