using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.ServiceInterfaces;
using TalabatAPI.DTOs;
using TalabatAPI.Extensions;

namespace TalabatAPI.Controllers
{

    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> usermanger;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> _usermanger, SignInManager<AppUser>_signInManager, ITokenService _tokenService,IMapper _mapper)
        {
            usermanger = _usermanger;
            signInManager = _signInManager;
            tokenService = _tokenService;
            mapper = _mapper;
        }

        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if ( CheckEmailExist(model.Email).Result.Value)
            {
                return BadRequest();
            }
            var User = new AppUser()
            {
                Email = model.Email,
                displayName = model.DisplayName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],
            };
            var Result = await usermanger.CreateAsync(User, model.Password); // Create Object of Database
            if (!Result.Succeeded)
            {
                return BadRequest();

            }  
            var ReturnResult = new UserDto()
            {
                Email = User.Email,
                DisplayName = User.displayName,
                Token =await tokenService.CreateTokenasync(User, usermanger)
            };
            return Ok(ReturnResult);

        }
        // LogIn  
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await usermanger.FindByEmailAsync(model.Email);
            if (User == null) { return Unauthorized(); }
           
           var Result =   await signInManager.CheckPasswordSignInAsync(User, model.Password,false);
            if (!Result.Succeeded) { return Unauthorized(); }
            var ReturnResult = new UserDto()
            {
                Email = User.Email,
                DisplayName = User.displayName,
                Token =await tokenService.CreateTokenasync( User, usermanger),
            };
            return Ok(ReturnResult);
        }


        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await usermanger.FindByEmailAsync(email);

            var Resultobject = new UserDto()
            {
                DisplayName = Result.displayName,
                Email = Result.Email,
                Token = await tokenService.CreateTokenasync(Result, usermanger)

            };
            return Ok(Resultobject);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            /* var email = User.FindFirstValue(ClaimTypes.Email);
              var Result = await usermanger.FindByEmailAsync(email);*/
           var user = await usermanger.FindUserByAddressAsync(User);
            var mappedAddress = mapper.Map<Address,AddressDto>(user.Address);
           return Ok(mappedAddress);

        }


        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdateData)
        {
            var user = await usermanger.FindUserByAddressAsync(User);
            UpdateData.Id = user.Address.Id;
            var MappedAddress = mapper.Map<AddressDto, Address>(UpdateData);
            user.Address = MappedAddress;
            var Result = await usermanger.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest();
            else return Ok(UpdateData);
        }

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>>CheckEmailExist(string Email)
        {
            var user= await usermanger.FindByEmailAsync(Email);
            if(user == null) return false;
            return true;

        }




    }
}
