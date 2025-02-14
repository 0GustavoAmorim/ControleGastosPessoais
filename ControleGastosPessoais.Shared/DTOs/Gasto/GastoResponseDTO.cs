namespace ControleGastosPessoais.Shared.DTOs.Gasto
{
    public class GastoResponseDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string CategoriaNome { get; set; }
    }
}
