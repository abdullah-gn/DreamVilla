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
	public class UserRepository : IUserRepository
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


		public async Task<TokenDto> Login(LoginRequestDto loginuserDto)
		{
			var user = db.applicationUsers.FirstOrDefault(n=>n.UserName.ToLower() == loginuserDto.UserName.ToLower());

			bool IsValid = await userManager.CheckPasswordAsync(user,loginuserDto.Password);



			if(user == null || IsValid == false)
			{
				return new TokenDto()
				{
					AccessToken = ""
				};
			}
			var jwtTokenId = $"JTI{Guid.NewGuid()}";
			var accessToken = await GetAccessToken(user,jwtTokenId);
			var refreshToken = await CreateRefreshToken(user.Id, jwtTokenId);

			TokenDto loginToken = new()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken,
            };
			return loginToken;			
		}

		public async Task<TokenDto> RefreshAccessToken(TokenDto tokenDto)
		{

			// Find an existing refresh Token
			var existingRefreshToken = await db.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDto.RefreshToken);
			if (existingRefreshToken == null)
			{
				return new TokenDto();
			}
			// Compare data from existing refresh and access token provided and if there is any mis-match then cosider it as fraud
			var isTokenValid = GetAccessTokenData(tokenDto.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
			if (!isTokenValid)
			{
				await MarkTokenAsInvalid(existingRefreshToken);
				return new TokenDto();

			}
			// When someone is access invalid token then return refuse it
			if (!existingRefreshToken.IsValid)
			{
				await MarkAllTokensInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
			}
	
			// If refresh token is expired then the user has to login in again
			if (existingRefreshToken.ExpiresAt<DateTime.UtcNow)
			{
				await MarkTokenAsInvalid(existingRefreshToken);
				return new TokenDto();
			}

			// If everthing is good then we replace the expired with new one and update the expire date
			var newRefreshToken = await CreateRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

			// Revoke existing refresh token
			 await MarkTokenAsInvalid(existingRefreshToken);

			// generate new access token
			var applicationUser = db.applicationUsers.FirstOrDefault(u=>u.Id == existingRefreshToken.UserId);
			if (applicationUser == null)
			{
				return new TokenDto();
			}
			var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

			return new TokenDto
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken
			};
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
		public async Task RevokeRefreshToken(TokenDto tokenDto)
		{
			var existingRefreshToken = await db.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDto.RefreshToken);
			if (existingRefreshToken == null)
				return;
			// Compare data from existing refresh and access token provided and
			// if there is any mis-match then we should do nothing with refresh token
			var isTokenValid = GetAccessTokenData(tokenDto.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
			if (!isTokenValid)
			{
				return;
			}
			await MarkAllTokensInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
		}
		private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
		{
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
					new Claim(ClaimTypes.Role, Roles.FirstOrDefault()),
					new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
					new Claim(JwtRegisteredClaimNames.Sub,user.Id)
				}),
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(TokenDescriptor);
			var tokenStr = tokenHandler.WriteToken(token);
			return tokenStr;
		}
		private bool GetAccessTokenData(string AccessToken, string expectedUserId, string excpectedTokenId)
		{
			try
			{
				var tokenHanlder = new JwtSecurityTokenHandler();
				var jwt = tokenHanlder.ReadJwtToken(AccessToken);
				var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
				var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;
				return userId==expectedUserId && excpectedTokenId == jwtTokenId;
			}
			catch(Exception e)
			{
				return false;
			}
		}
		private async Task<string> CreateRefreshToken(string userId, string jwtTokenId)
		{
			RefreshToken refreshToken = new RefreshToken
			{
				IsValid = true,
				UserId = userId,
				JwtTokenId = jwtTokenId,
				ExpiresAt = DateTime.UtcNow.AddDays(30),
				Refresh_Token = $"{Guid.NewGuid()}" + "-" + Guid.NewGuid()
			};
			await db.RefreshTokens.AddAsync(refreshToken);
			await db.SaveChangesAsync();
			return refreshToken.Refresh_Token;

		}
		private async Task MarkAllTokensInChainAsInvalid(string userId, string tokenId)
		{
			await db.RefreshTokens.Where(u => u.UserId == userId
					&& u.JwtTokenId == tokenId)
					.ExecuteUpdateAsync(u => u.SetProperty(refresh => refresh.IsValid, false));
		}
		private Task MarkTokenAsInvalid(RefreshToken refreshToken)
		{
			refreshToken.IsValid = false;
			return db.SaveChangesAsync();
		}

	}
	
}
