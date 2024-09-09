using AutoMapper;
using Talabat.Core.Entities;
using TalabatAPIProject.Dtos;

namespace TalabatAPIProject.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration Configuration;
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{Configuration["ApiUrl"]}{source.PictureUrl}";
            return string.Empty;
        }
    }
}
