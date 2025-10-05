using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class BeneficiarioModel
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow; // Padrão ser a data atual
        public Status Status { get; set; } = Status.ATIVO; // Padrão ser ativo

        public int PlanoId { get; set; } //FK

        public PlanoModel Plano { get; set; } //prop de navegação
    }

}
