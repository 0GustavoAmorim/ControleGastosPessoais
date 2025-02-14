using System.ComponentModel.DataAnnotations;

namespace ControleGastosPessoais.Shared.DTOs.Gasto
{
    public class GastoRequestDTO
    {
        [Required(ErrorMessage = "A descrição do gasto é obrigatória.")]
        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
        [MinLength(3, ErrorMessage = "O nome da categoria deve ter no mínimo 3 caracteres.")]
        public string Descricao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do gasto não pode ser zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Informar Categoria do gasto é obrigatória.", AllowEmptyStrings = false)]
        public int CategoriaId { get; set; }
    }

}
