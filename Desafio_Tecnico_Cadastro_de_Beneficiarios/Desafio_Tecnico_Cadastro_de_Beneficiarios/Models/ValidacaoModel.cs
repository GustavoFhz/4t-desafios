namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class ValidacaoModel
    {
        public string Field { get; set; } // Nome do campo que falhou na validação
        public string Rule { get; set; } // Regra de validação que foi violada
    }
}
