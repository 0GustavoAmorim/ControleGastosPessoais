using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGastosPessoais.Shared.Models;

public class Gasto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(100, ErrorMessage = "A descrição deve ter no máximo 100 caracteres")]
    [Column(TypeName = "VARCHAR")]
    public string Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    [Required]
    public DateTime? Data { get; set; } = DateTime.Now;

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria? Categoria { get; set; }
}
