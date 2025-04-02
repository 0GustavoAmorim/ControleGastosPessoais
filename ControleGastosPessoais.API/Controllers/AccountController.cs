using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.Shared.Models;
using ControleGastosPessoais.Shared.Models.AuthenticationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosPessoais.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager
                                ,AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Ok(new RegisterResult { Successful = false, Errors = errors });
            }

            var categoriaPadrao = new Categoria
            {
                Nome = "Sem Categoria",
                UserId = newUser.Id
            };

            _context.Categorias.Add(categoriaPadrao);
            await _context.SaveChangesAsync();

            return Ok(new RegisterResult { Successful = true });
        }
    }
 }
