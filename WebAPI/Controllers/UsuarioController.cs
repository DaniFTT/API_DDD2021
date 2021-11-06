using Application.Interfaces;
using Entidades.Entities;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IApplicationUsuario _applicationUsuario;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsuarioController(IApplicationUsuario applicationUsuario, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _applicationUsuario = applicationUsuario;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/CriaToken")]
        public async Task<IActionResult> CriaToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Unauthorized();

            var result = await _applicationUsuario.ExisteUsuario(login.Email, login.Senha);
            if (result)
            {
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-1235678"))
                    .AddSubject("Empresa - Daniel Santos")
                    .AddIssuer("Teste.Security.Bearer")
                    .AddAudience("Teste.Security.Bearer")
                    .AddClaim("UsuarioAPINumero", "1")
                    .AddExpiry(5)
                    .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/AdicionaUsuario")]
        public async Task<IActionResult> AdicionaUsuario([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha) || string.IsNullOrWhiteSpace(login.Celular) || login.Idade < 0)
                return BadRequest("Faltam alguns dados!");

            var userExists = await _applicationUsuario.ExisteUsuario(login.Email, login.Senha);

            if (!userExists)
            {
                var result = await
                    _applicationUsuario.AdicionaUsuario(login.Email, login.Senha, login.Idade, login.Celular);

                if (result)
                    return Ok("Usuário Adicionado com Sucesso!");
                else
                    return BadRequest("Erro ao adicionar o usuário");
            }


            return BadRequest("Usuário já Existe!");
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/CriaTokenIdentity")]
        public async Task<IActionResult> CriaTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                return Unauthorized();

            var result = await
                _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = new TokenJWTBuilder()
                   .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-1235678"))
                   .AddSubject("Empresa - Daniel Santos")
                   .AddIssuer("Teste.Security.Bearer")
                   .AddAudience("Teste.Security.Bearer")
                   .AddClaim("UsuarioAPINumero", "1")
                   .AddExpiry(5)
                   .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("api/AdicionaUsuarioIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha) || string.IsNullOrWhiteSpace(login.Celular) || login.Idade < 0)
                return BadRequest("Faltam alguns dados!");

            var user = new ApplicationUser
            {
                UserName = login.Email,
                Email = login.Email,
                Celular = login.Celular,
                Idade = login.Idade,
                Tipo = TipoUsuario.Comum
            };

            var result = await _userManager.CreateAsync(user, login.Senha);

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
