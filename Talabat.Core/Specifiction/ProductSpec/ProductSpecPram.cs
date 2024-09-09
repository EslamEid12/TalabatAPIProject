using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifiction.ProductSpec
{
    public class ProductSpecPram
    {
        private const int MaxPageSize = 10;
        private  int pageSize = 5; 

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public int PageIndex { get; set; }
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? CategoryId { get; set; }

    }
}
