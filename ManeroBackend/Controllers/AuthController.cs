﻿using ManeroBackend.Authentication;
using ManeroBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ManeroBackend.Services;
using System;

namespace ManeroBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IGoogleTokenService _googleTokenService;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IGoogleTokenService googleTokenService, IUserService userService)
        {
            _configuration = configuration;

            _googleTokenService = googleTokenService;
            _userService = userService;
        }







        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleAuth([FromBody] AuthRequest request)
        {
            try
            {
                Console.WriteLine($"Received request: {JsonConvert.SerializeObject(request)}");

                // Use the GoogleTokenService to exchange the code for a token
                var tokenResponse = await _googleTokenService.ExchangeCodeForTokenAsync(new TokenRequest { Code = request.Code });

                if (tokenResponse.StatusCode != Enums.StatusCode.Ok)
                {
                    Console.WriteLine(tokenResponse.Message);
                    return BadRequest(tokenResponse.Message);
                }

                var tokenObject = JsonConvert.DeserializeObject<TokenResponse>(tokenResponse.Message);
                var accessToken = tokenObject!.Access_token;

                // Fetch user info using the token
                var googleUserResponse = await _googleTokenService.GetGoogleUserFromTokenAsync(accessToken.ToString());


                if (googleUserResponse.StatusCode != Enums.StatusCode.Ok || googleUserResponse.Content == null)
                {
                    return BadRequest(new { error = "Invalid Google Token." });
                }



                var googleUser = googleUserResponse.Content;

                var existingUserResponse = await _userService.GetUserByEmailAsync(googleUser.Email);

                // If the user already exists, return their details (or however you want to handle it)
                if (existingUserResponse.StatusCode == Enums.StatusCode.Ok && existingUserResponse.Content != null)
                {
                    return Ok(existingUserResponse.Content);
                }

                // If user does not exist, create a new user.
                var newUserResponse = await _googleTokenService.CreateGoogleUserAsync(googleUser);
                Console.WriteLine($"Create User Status Code: {newUserResponse.StatusCode}");
                Console.WriteLine($"Create User Message: {newUserResponse.Message}");



                if (newUserResponse.StatusCode == Enums.StatusCode.Created)
                {
                    return CreatedAtAction(nameof(GoogleAuth), newUserResponse.Content);
                }

                return BadRequest("Could not create user!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during token verification: {ex.Message}");
                Console.WriteLine(ex.ToString());
                return BadRequest(new { error = "Error occurred during user creation." });

            }
        }




    }
}
