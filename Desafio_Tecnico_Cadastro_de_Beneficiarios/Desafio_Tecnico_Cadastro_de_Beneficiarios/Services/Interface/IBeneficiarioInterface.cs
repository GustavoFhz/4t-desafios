using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface
{
    public interface IBeneficiarioInterface
    {
        Task<ResponseModel<BeneficiarioModel>> CriarBeneficiario(BeneficiarioCriacaoDto beneficiarioCriacaoDto); 
        Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios(); 
        Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id); 
        Task<ResponseModel<BeneficiarioModel>> EditarBeneficiarios(BeneficiarioEdicaoDto beneficiarioEdicaoDto); 
        Task<ResponseModel<BeneficiarioModel>> DeletarBeneficiario(int id); 
    }
}
