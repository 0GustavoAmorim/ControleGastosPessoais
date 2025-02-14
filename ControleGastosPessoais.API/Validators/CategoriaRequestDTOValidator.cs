using ControleGastosPessoais.Shared.DTOs.Categoria;
using FluentValidation;

namespace ControleGastosPessoais.API.Validators
{
    public class CategoriaRequestDTOValidator : AbstractValidator<CategoriaRequestDTO>
    {
        public CategoriaRequestDTOValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(50).WithMessage("O nome da categoria deve ter no máximo 50 caracteres.")
                .MinimumLength(3).WithMessage("O nome da categoria deve ter no mínimo 3 caracteres.");
        }
    }
}
