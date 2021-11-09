using Application.Interfaces;
using Entidades.Entities;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IApplicationUsuario _applicationUsuario;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsuarioController(IApplicationUsuario applicationUsuario, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration Configuration)
        {
            _applicationUsuario = applicationUsuario;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = Configuration;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriaToken")]
        public async Task<IActionResult> CriaToken([FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Unauthorized();

            var result = await _applicationUsuario.ExisteUsuario(login.Email, login.Senha);
            if (result)
            {
                var userId = await _applicationUsuario.RetornaIdUsuario(login.Email);
                var claims = new List<Claim>();
                claims.Add(new Claim("UserId", userId));

                var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var _issuer = _config["Jwt:Issuer"];
                var _audience = _config["Jwt:Audience"];
                var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(tokenString);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionaUsuario")]
        public async Task<IActionResult> AdicionaUsuario([FromBody] RegisterModel register)
        {
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Senha) || string.IsNullOrWhiteSpace(register.Celular) || register.Idade < 0)
                return BadRequest("Faltam alguns dados!");

            var userExists = await _applicationUsuario.ExisteUsuario(register.Email, register.Senha);

            if (!userExists)
            {
                var result = await
                    _applicationUsuario.AdicionaUsuario(register.Email, register.Senha, register.Idade, register.Celular);

                if (result)
                    return Ok("Usuário Adicionado com Sucesso!");
                else
                    return BadRequest("Erro ao adicionar o usuário");
            }


            return BadRequest("Usuário já Existe!");
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriaTokenIdentity")]
        public async Task<IActionResult> CriaTokenIdentity([FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Unauthorized();

            var result = await
                _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var userId = await _applicationUsuario.RetornaIdUsuario(login.Email);
                var claims = new List<Claim>();
                claims.Add(new Claim("UserId", userId));

                var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var _issuer = _config["Jwt:Issuer"];
                var _audience = _config["Jwt:Audience"];
                var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(tokenString);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionaUsuarioIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] RegisterModel register)
        {
            if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Senha) || string.IsNullOrWhiteSpace(register.Celular) || register.Idade < 0)
                return BadRequest("Faltam alguns dados!");

            var user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                Celular = register.Celular,
                Idade = register.Idade,
                Tipo = TipoUsuario.Comum
            };

            var result = await _userManager.CreateAsync(user, register.Senha);

            if (result.Errors.Any())
                return BadRequest(result.Errors);

            //Geração de Confirmação caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //Retorno email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultConfirmationEmail = await _userManager.ConfirmEmailAsync(user, code);

            if (resultConfirmationEmail.Succeeded)
                return Ok("Usuário Adicionado com Sucesso!");
            else
                return BadRequest("Erro ao confirmar usuário!");
        }
    }
}
