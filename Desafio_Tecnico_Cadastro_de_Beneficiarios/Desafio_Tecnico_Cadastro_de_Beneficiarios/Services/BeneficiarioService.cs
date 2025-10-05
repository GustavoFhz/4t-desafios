using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services
{
    
    public class BeneficiarioService : IBeneficiarioInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        //Injeção de dependência do contexto do banco de dados e do mapper
        public BeneficiarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Busca do beneficiário pelo id, caso não encontrado envia uma mensagem de não encontrado 
        public async Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if(beneficiario == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiario não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "not_found"
                    });

                    return response;
                }
                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário localizado com sucesso";
                return response;

            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }
        //Cria o beneficiário com as informações passados pelo dto de criação
        //valida o cpf antes de criar o usuario
        //Faz uma validação pelo cpf se já existe o mesmo cpf no banco de dados
        //Faz uma verificação do plano de saúde para verificar se existe
        public async Task<ResponseModel<BeneficiarioModel>> CriarBeneficiario(BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            var response = new ResponseModel<BeneficiarioModel>();

            try
            {
                if (!CpfValido(beneficiarioCriacaoDto.Cpf)) //validação do cpf
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "CPF inválido";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "cpf",
                        Rule = "invalid"
                    });
                    return response;
                }
                if (BeneficiarioExiste(beneficiarioCriacaoDto)) //validação do beneficiario
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "CPF já cadastrado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "cpf",
                        Rule = "duplicate"
                    });
                    return response;
                }

                var plano = await _context.Planos.FindAsync(beneficiarioCriacaoDto.PlanoId);  //verifica se o plano existe
                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "NotFound";
                    response.Mensagem = "Plano não encontrado";
                    return response;
                }


                BeneficiarioModel beneficiario = _mapper.Map<BeneficiarioModel>(beneficiarioCriacaoDto);

                _context.Add(beneficiario);
                await _context.SaveChangesAsync();

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário criado com sucesso";
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


        //Remove o beneficiário pelo id 
        public async Task<ResponseModel<BeneficiarioModel>> DeletarBeneficiario(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if(beneficiario == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiario não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;
                }

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário removido com sucesso";

                _context.Beneficiarios.Remove(beneficiario);
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

        //Edita o beneficiário pelo id passado no dto de edição
        public async Task<ResponseModel<BeneficiarioModel>> EditarBeneficiarios(BeneficiarioEdicaoDto beneficiarioEdicaoDto)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiarioBanco = await _context.Beneficiarios.FindAsync(beneficiarioEdicaoDto.Id);

                if (beneficiarioBanco == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "CPF inválido";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "cpf",
                        Rule = "invalid"
                    });

                    return response;
                }
                beneficiarioBanco.NomeCompleto = beneficiarioEdicaoDto.NomeCompleto;
                beneficiarioBanco.Cpf = beneficiarioEdicaoDto.Cpf;
                beneficiarioBanco.DataNascimento = beneficiarioEdicaoDto.DataNascimento;
                beneficiarioBanco.Status = beneficiarioEdicaoDto.Status;

                _context.Update(beneficiarioBanco);
                await _context.SaveChangesAsync();
                
                response.Dados = beneficiarioBanco;
                response.Mensagem = "Beneficiário editado com sucesso";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "NotFound";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        //Lista todos os beneficiários encontrados no banco de dados 
        public async Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios()
        {
            ResponseModel<List<BeneficiarioModel>> response = new ResponseModel<List<BeneficiarioModel>>();

            try
            {
                var beneficiarios = await _context.Beneficiarios.ToListAsync();

                response.Dados = beneficiarios;
                response.Mensagem = "Beneficiários listados com sucesso";
                return response;


            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        //Função para verificar se existe o beneficiário pelo cpf no banco de dados
        public bool BeneficiarioExiste(BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            return _context.Beneficiarios.Any(item => item.Cpf == beneficiarioCriacaoDto.Cpf);
        }

        //Função para verificar se o cpf é valido
        private bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false; //verifica se é nulo ou vazio


            cpf = cpf.Replace(".", "").Replace("-", ""); //remove pontos e traços

            if (cpf.Length != 11) return false; //verifica se tem 11 digitos
            if (new string(cpf[0], 11) == cpf) return false; //verifica se todos os digitos são iguais

            return true;

        }
    }
}
