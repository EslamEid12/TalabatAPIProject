using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using TalabatAPIProject.Dtos;
using TalabatAPIProject.Error;
using TalabatAPIProject.Extension;

namespace TalabatAPIProject.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signIn;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> user,
            SignInManager<AppUser> signIn
            ,IAuthService authService,
            IMapper mapper)
        {
            _userManager = user;
            _signIn = signIn;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LogIn(LoginDto modle)
        {
            var user = await _userManager.FindByEmailAsync(modle.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));
            var Result = await _signIn.CheckPasswordSignInAsync(user, modle.Password, false);
            if (Result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto modle)
        {
            if (CheakEmailExists(modle.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors= new string[] {"this email is Already exist" } });

            var user = new AppUser()
            {
                DisplayName = modle.DisplayName,
                Email = modle.Email,
                UserName = modle.Email.Split("/@")[0],
                PhoneNumber = modle.PhoneNumber
            };
            var Result = await _userManager.CreateAsync(user, modle.Password);
            if (Result.Succeeded is false)
                return BadRequest(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });


        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var email= User.FindFirstValue(ClaimTypes.Email);
            var user =await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var address= _mapper.Map<AddressDto>(user.Address);
            return Ok( address);
        }

         [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updateAddress)
        {

            var address = _mapper.Map<AddressDto, Talabat.Core.Entities.Identity.Address>(updateAddress);
            var user = await _userManager.FindUserWithAddressAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded)
                return BadRequest( new ApiResponse(400));
            return Ok( updateAddress);
        }
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>>CheakEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null; 
        }

    }
}
