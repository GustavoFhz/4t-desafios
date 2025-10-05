namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class ResponseModel<T>
    {
        public T Dados { get; set; } // Dados do tipo genérico T
        public string Error { get; set; }
        public string Mensagem { get; set; }
        public bool Status { get; set; } = true;
        public List<ValidacaoModel> Details { get; set; } = new List<ValidacaoModel>(); // Inicializa a lista vazia dos detalhes de validação
    }
}
