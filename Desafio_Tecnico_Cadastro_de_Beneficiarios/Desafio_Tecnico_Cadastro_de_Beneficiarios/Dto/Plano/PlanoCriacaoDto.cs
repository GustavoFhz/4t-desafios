using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano
{
    public class PlanoCriacaoDto
    {
        [Required(ErrorMessage = " O  nome do plano é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O código de registro da ANS é obrigatório")]
        public string Codigo_registro_ans { get; set; }
    }
}
