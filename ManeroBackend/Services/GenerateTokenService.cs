using ManeroBackend.Authentication;
using ManeroBackend.Contexts;
using ManeroBackend.Enums;
using ManeroBackend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManeroBackend.Services;

public class GenerateTokenService
{
    private readonly IOptions<JwtSettings> _jwtSettings;

    public GenerateTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }
    public string GenerateJWTForUser(string userId)
    {
        var secret = _jwtSettings.Value.Key;
        // If your secret is base64 encoded in the configuration, decode it
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D3fP67Zn2v9w4x!Bb5tGhQmR8jLk6YnR"));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
          new Claim("additionalAudiences", JsonConvert.SerializeObject(new List<string> { "https://localhost:7076" })),
    };

        var tokenOptions = new JwtSecurityToken(
                       issuer: "https://localhost:7277",
                       audience: "https://localhost:7277",
                       claims: claims,
                       expires: DateTime.Now.AddMinutes(5),
                       signingCredentials: signinCredentials
                   );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }


    public Task<ServiceResponse<string>> CreateUserAndReturnToken(ApplicationUser newUser)
    {
        var token = GenerateJWTForUser(newUser.Id.ToString());
        return Task.FromResult(new ServiceResponse<string>
        {
            StatusCode = Enums.StatusCode.Ok,
            Content = token
        });
    }


   

}

