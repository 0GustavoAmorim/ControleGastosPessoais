using System.ComponentModel.DataAnnotations;

namespace ControleGastosPessoais.Shared.DTOs.Categoria
{
    public class CategoriaRequestDTO
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da categoria deve ter no máximo 50 caracteres.")]
        [MinLength(3, ErrorMessage = "O nome da categoria deve ter no mínimo 3 caracteres.")]
        public string Nome { get; set; }
    }
}
