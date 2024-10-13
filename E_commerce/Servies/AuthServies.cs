using EcomMakeUp.Dtos;
using EcomMakeUp.Helpers;
using EcomMakeUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace EcomMakeUp.Servies
{
    public class AuthServies : IAuthServies
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthServies(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> CheckUserLogin(LoginDTO UserData)
        {
            var Auth = new AuthModel();
            var user=await _userManager.FindByEmailAsync(UserData.Email);
            if(user == null || !await _userManager.CheckPasswordAsync(user, UserData.Password))
            {
                Auth.Message = "Error in Email or Password";
                return Auth;
            }
            var jwtToken = await CreateToken(user);
            Auth.IsAuthenticated = true;
            Auth.Email = UserData.Email;
            Auth.Role = new List<string> { "User" };
            Auth.Token=new JwtSecurityTokenHandler().WriteToken(jwtToken);

            if(user.refreshTokens.Any(t=>t.IsActive))
            {
                var activeRefreshToken = user.refreshTokens.FirstOrDefault(t => t.IsActive);
                Auth.RefreshToken = activeRefreshToken.Token;
                Auth.RefreshTokenExpirat = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                Auth.RefreshToken = refreshToken.Token;
                Auth.RefreshTokenExpirat = refreshToken.ExpiresOn;
                user.refreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
          
            return Auth;
        }

        public async Task<AuthModel> createUser(ApplicationUser user , string password)
        {
            var user1=await _userManager.FindByEmailAsync(user.Email);
            if( user1!= null)
            {
                return new AuthModel {
                    Message = "Email is already registered" ,
                    IsAuthenticated = false
                };
            }
          var  result= await _userManager.CreateAsync(user , password);
            if(!result.Succeeded) { 
              var errors=string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
               return new AuthModel { Message = errors };
            }
           await _userManager.AddToRoleAsync(user, "User");
            var jwtToken=await CreateToken(user);

            return new AuthModel
            {
                Email = user.Email,
               // Expireson = jwtToken.ValidTo,
                IsAuthenticated = true,
                IdUser=user.Id,
                Role = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),

            };
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            // Get user claims and roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            // Create claims for user roles
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("role", role));
            }

            // Combine all claims
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
        new Claim("uid", user.Id)  // Custom claim for user ID
    }
            .Union(userClaims) // Add user-specific claims
            .Union(roleClaims); // Add role-specific claims

            // Generate a symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)); // Replace with your actual secret key

            // Create signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,  // Replace with your issuer
                audience: _jwt.Audience,  // Replace with your audience
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),  // Set the expiration time
                signingCredentials: signingCredentials
            );

            return token;
        }

        public ApplicationUser deleteUser(ApplicationUser user)
        {
           _userManager.DeleteAsync(user);
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> listUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> listUsersByName(string name)
        {
            return  await _userManager.Users.Where(x=>x.FullName.Contains(name)).ToListAsync();
        }

        public async Task<ApplicationUser> SelectUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> SelectUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> AddRole(AddRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if(user is null || !await _roleManager.RoleExistsAsync(dto.RoleName))
            {
                return ("Error in UserIs Or Role");
            }

            if (await _userManager.IsInRoleAsync(user, dto.RoleName))
                return ("This role aready exist");

            var res = await _userManager.AddToRoleAsync(user, dto.RoleName);
            return res.Succeeded ? string.Empty : "Please try agen";
        }

        public async Task<AuthModel> changePassword(ChangePasswordDTO dto)
        {
            var user = await SelectUserByEmail(dto.Email);
            if(user == null  || !await _userManager.CheckPasswordAsync(user , dto.OldPassword) || dto.ConNewPass != dto.NewPassword)
            {
                return new AuthModel { Message = "You have error in data" };
            }

            

            var res = await _userManager.ChangePasswordAsync(user, dto.OldPassword ,dto.NewPassword);

            if(!res.Succeeded)
            {
                return new AuthModel { Message = "You have error in change password" };
            }

            return new AuthModel { Email=dto.Email };
        }

        public async Task<AuthModel> updateUser(ApplicationUser user)
        {
            var res = await  _userManager.UpdateAsync(user);

            if(!res.Succeeded)
            {
                return new AuthModel { Message = "Error in update" };
            }

            return new AuthModel { };
        }


        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber= new byte[32];
            using var generator = new RNGCryptoServiceProvider();
              generator.GetBytes(randomNumber);

            return new RefreshToken
            {
               Token = Convert.ToBase64String(randomNumber),
               ExpiresOn = DateTime.UtcNow.AddDays(value:10),
               CreatedOn = DateTime.UtcNow,

            };
        
        }
    }
}
