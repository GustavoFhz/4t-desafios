using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Profiles
{
    public class PlanoProfile : Profile
    {
        //Mapeamento do DTO de criação para o modelo de Plano 
        public PlanoProfile()
        {
            CreateMap<PlanoCriacaoDto, PlanoModel>();
        }
    }
}
