using AutoMapper;
using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Models.Dto;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DreamVilla_VillaApi.Repository
{
	public class UserRepository : ILocalUserRepository
	{
		private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapping;
        private readonly RoleManager<IdentityRole> roleManager;
        private string SecretKey;
		public UserRepository(ApplicationDbContext _Db , IConfiguration _Config , UserManager<ApplicationUser> _UserManager,
			IMapper _mapper , RoleManager<IdentityRole> _RoleManager)
        {
			db = _Db;
            userManager = _UserManager;
			mapping = _mapper;
            roleManager = _RoleManager;
            this.SecretKey = _Config.GetValue<string>("ApiSettings:SecretKey");

		}


        public bool IsUniqueUser(string username)
		{
			var user = db.applicationUsers.FirstOrDefault(n=>n.UserName == username);
			if(user == null)
			{
				return true;
			}
			return false;
		}


		public async Task<LoginResponseDto> Login(LoginRequestDto loginuserDto)
		{
			var user = db.applicationUsers.FirstOrDefault(n=>n.UserName.ToLower() == loginuserDto.UserName.ToLower());

			bool IsValid = await userManager.CheckPasswordAsync(user,loginuserDto.Password);



			if(user == null || IsValid == false)
			{
				return new LoginResponseDto()
				{
					Token = "",
					User = null
				};
			}
			//Generate JWT token
			//Token handler > JWTSecurity TokenHandler
			//Token descriptor
			//subject > array of identityClaims > Name , Role
			//Expires
			//SingingCredentials > Key , Algo

			var Roles = await userManager.GetRolesAsync(user);

			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.ASCII.GetBytes(SecretKey);

			var TokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName.ToString()),
					new Claim(ClaimTypes.Role, Roles.FirstOrDefault())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(TokenDescriptor);

			LoginResponseDto loginResponseDto = new()
			{
				Token = tokenHandler.WriteToken(token),
				User = mapping.Map<AppUserDto>(user),
            };
			return loginResponseDto;			
		}


		
		public async Task<AppUserDto> Register(RegisterDto registeruserDto)
		{
			ApplicationUser User = null;

			if(registeruserDto.UserName != null && registeruserDto.Password !=null)
			{
				User = new()
				{
					UserName = registeruserDto.UserName,
					Email = registeruserDto.UserName,
					NormalizedEmail = registeruserDto.UserName.ToUpper(),
					Name = registeruserDto.Name,
				};
			}
			

			try
			{
                var Result = await userManager.CreateAsync(User, registeruserDto.Password);

                if (Result.Succeeded)
                {
					if (!roleManager.RoleExistsAsync(registeruserDto.Role).GetAwaiter().GetResult())
					{
						await roleManager.CreateAsync(new IdentityRole(registeruserDto.Role));
					
                    }

                    await userManager.AddToRoleAsync(User, registeruserDto.Role);
					var ReturnUser = db.applicationUsers.FirstOrDefault(n=>n.UserName == registeruserDto.UserName);
                    return mapping.Map<AppUserDto>(ReturnUser);
                }
                else {
                    AppUserDto AppDto = new();

					AppDto.Errors = JsonSerializer.Serialize(Result.Errors.Select(e => e.Description)) ;
					return AppDto;
                }

            }	
            catch (Exception ex)
            {
				
            }
			return new AppUserDto();
		}		

	}
	
}
