using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Repository.Data;
using TalabatAPIProject.Error;

namespace TalabatAPIProject.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        private readonly StoreDBContext _dBContext;

        public BuggyController(StoreDBContext dBContext) {
            _dBContext = dBContext;
        }    
        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest() 
        {
            var product = _dBContext.Products.Find(100);
            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
                   
        }
        [HttpGet("servereerror")]
        public IActionResult GetServerError()
        {
            var product =_dBContext.Products.Find(100);
            var productToReturn=product.ToString();
            return Ok(productToReturn);

        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest{id}")]
        public IActionResult GetBadRequest(int id)
        {
            return Ok ();
        }
        [HttpGet("unauthorized")]
        public IActionResult GetUnAuthorizedError()
        {
            return Unauthorized(new ApiResponse(401));
        }

       
    }
}
