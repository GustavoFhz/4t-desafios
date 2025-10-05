using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services
{
    public class PlanoService : IPlanoInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        //Injeção de dependência do contexto do banco de dados e do mapper
        public PlanoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Busca do plano de saúde pelo id, caso não encontrado envia uma mensagem de não encontrado no response
        public async Task<ResponseModel<PlanoModel>> BuscarPlanoPorId(int id)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var plano = await _context.Planos.FindAsync(id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });

                    return response;
                }

                response.Dados = plano;
                response.Mensagem = "Plano localizado com sucesso";
                return response;
            }

            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;

            }
        }

        //Cria o plano de saúde com as informações passados pelo dto de criação de plano
        //Faz uma verificação pelo nome do plano, caso já exista um plano com o mesmo nome, não permite a criação
        public async Task<ResponseModel<PlanoModel>> CriarPlano(PlanoCriacaoDto planoCriacaoDto)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                if (PlanoExiste(planoCriacaoDto))
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano já criado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });

                    return response;
                }

                PlanoModel plano = _mapper.Map<PlanoModel>(planoCriacaoDto);

                _context.Add(plano);
                await _context.SaveChangesAsync();

                response.Dados = plano;
                response.Mensagem = "Plano criado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        //Remove o plano de sáude pelo id
        public async Task<ResponseModel<PlanoModel>> DeletarPlano(int id)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var plano = await _context.Planos.FindAsync(id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;

                }
                response.Dados = plano;
                response.Mensagem = "Plano removido com sucesso";

                _context.Planos.Remove(plano);
                await _context.SaveChangesAsync();

                

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        //Edita o plano de saúde pelo id passado no dto de edição de plano
        public async Task<ResponseModel<PlanoModel>> EditarPlano(PlanoEdicaoDto planoEdicaoDto)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var PlanoBanco = _context.Planos.Find(planoEdicaoDto.Id);

                if (PlanoBanco == null)
                {
                    response.Status = false;
                    response.Mensagem = "Plano não localizado";
                    response.Error = "ValidationError";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;
                }

                PlanoBanco.Nome = planoEdicaoDto.Nome;
                PlanoBanco.Codigo_registro_ans = planoEdicaoDto.Codigo_registro_ans;

                _context.Planos.Update(PlanoBanco);
                await _context.SaveChangesAsync();

                response.Dados = PlanoBanco;
                response.Mensagem = "Plano editado com sucesso";
                return response;

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }


        //Faz a listagem dos planos de saúde encontrados no banco de dados
        public async Task<ResponseModel<List<PlanoModel>>> ListarPlanos()
        {
            ResponseModel<List<PlanoModel>> response = new ResponseModel<List<PlanoModel>>();

            try
            {
                var planos = await _context.Planos.ToListAsync();

                response.Dados = planos;
                response.Mensagem = "Planos listados com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }


        //Verifica se existe um plano com o mesmo nome (Sempre possivel a criação de apenas um plano com o mesmo nome)
        public bool PlanoExiste(PlanoCriacaoDto planoCriacaoDto)
        {
            return _context.Planos.Any(item => item.Nome == planoCriacaoDto.Nome);
        }
    }
}
