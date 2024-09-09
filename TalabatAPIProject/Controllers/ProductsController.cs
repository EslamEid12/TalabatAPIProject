using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Core.Specifiction;
using Talabat.Core.Specifiction.ProductSpec;
using TalabatAPIProject.Dtos;
using TalabatAPIProject.Error;
using TalabatAPIProject.Helpers;

namespace TalabatAPIProject.Controllers
{
   
    public class ProductsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        ///private readonly IGenaricRepository<Product> _ProductRepo;
        ///private readonly IGenaricRepository<ProductBrand> _prandRepo;
        ///private readonly IGenaricRepository<ProductType> _typeRepo;

        public ProductsController(
              IMapper mapper,
              IProductService productService
            ///IGenaricRepository<Product>ProductRepo,
            ///IGenaricRepository<ProductBrand>PrandRepo,
            ///IGenaricRepository<ProductType>TypeRepo)
            )
        {
            _mapper = mapper;
            _productService = productService;
            ///_ProductRepo = ProductRepo;
            ///_prandRepo = PrandRepo;
            ///_typeRepo = TypeRepo;
        }
        //[Authorize/*(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)*/]
        [CachedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProduct([FromQuery]ProductSpecPram specPram)
        {
            var product = await _productService.GetProductAsync(specPram);
            if (product == null)
                return NotFound(new ApiResponse(404));
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(product);
            var count = await _productService.GetCountAsync(specPram);
            return Ok(new Pagination<ProductToReturnDto>(specPram.PageSize,specPram.PageIndex,count,data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
           
                var product = await _productService.GetProductAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
           
            }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrand()
        {
            return Ok(await _productService.GetProductBrand()); 
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetCategory()
        {
            return Ok(await _productService.GetProductType());
        }
    }
}
