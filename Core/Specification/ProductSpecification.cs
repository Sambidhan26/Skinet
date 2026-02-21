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
        public ProductSpecification(ProductSpecificationParam specParam): base(u => 
        (string.IsNullOrEmpty(specParam.Search) || u.Name.ToLower().Contains(specParam.Search)) &&
        (specParam.Brands.Count == 0 || specParam.Brands.Contains(u.Brand) && 
        (specParam.Types.Count == 0|| specParam.Types.Contains(u.Type))
        ))
        {
            ApplyPaging(specParam.PageSize * (specParam.PageIndex - 1), specParam.PageSize);
            switch (specParam.Sort)
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
