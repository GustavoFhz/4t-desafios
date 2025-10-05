namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class PlanoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo_registro_ans { get; set; }
        public ICollection<BeneficiarioModel> Beneficiarios { get; set; } // Um plano para vários beneficiarios
        
    }
}
