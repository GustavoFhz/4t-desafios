using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Profiles
{
    public class BeneficiarioProfile : Profile
    {
        //Mapeamento do DTO de criação para o modelo de Beneficiário
        public BeneficiarioProfile()
        {
            CreateMap<BeneficiarioCriacaoDto, BeneficiarioModel>();
        }
    }
}
