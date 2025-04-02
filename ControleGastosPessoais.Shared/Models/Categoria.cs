using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGastosPessoais.Shared.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome para a Categoria é obrigatório")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    [Column(TypeName = "VARCHAR")]
    public string Nome { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public List<Gasto> Gastos { get; set; } = new List<Gasto>();


}
