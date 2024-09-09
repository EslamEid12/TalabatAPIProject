using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using TalabatAPIProject.Dtos;
using TalabatAPIProject.Error;

namespace TalabatAPIProject.Controllers
{
    
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]  //Get : /api/basket?id=
        public async Task<ActionResult<CustomerBasket>>GetBasket(string id)
        {
            var basket=await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost]//Post : /api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedbasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreatedOrUpdateBasket=await _basketRepository.UpdateBasketAsync(mappedbasket);
            if (CreatedOrUpdateBasket is null)
                return BadRequest(new ApiResponse(400));      
            return Ok(CreatedOrUpdateBasket);
        }
        [HttpDelete] //Delete : /api/basket?id=
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

        }
    }
}
