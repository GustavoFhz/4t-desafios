using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario
{
    public class BeneficiarioCriacaoDto
    {

        [Required(ErrorMessage = "O nome completo é obrigatório")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [RegularExpression(@"(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})", ErrorMessage = "CPF deve estar no formato 000.000.000-00 ou 00000000000")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        public int PlanoId { get; set; }
    }
}
