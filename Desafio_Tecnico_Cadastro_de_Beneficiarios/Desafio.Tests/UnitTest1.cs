using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace Desafio.Tests //Testes usando o AAA (Arrange,Act,Assert)
{
    public class BeneficiarioTests : IDisposable 
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeneficiarioTests()
        {
            //Cria��o de um banco de dados inMemory
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // configura��o do automapper

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BeneficiarioCriacaoDto, BeneficiarioModel>();
                cfg.CreateMap<BeneficiarioEdicaoDto, BeneficiarioModel>();
            });
            _mapper = config.CreateMapper();
        }

        //serve para limpar o banco ap�s cada execu��o
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // HELPERS

        // Cria servi�o com AutoMapper e contexto
        private BeneficiarioService CriarService() => new BeneficiarioService(_context, _mapper);

        // Cria servi�o j� com um benefici�rio existente (para testar duplicidade)
        private async Task<BeneficiarioService> CriarServiceComBeneficiarioExistente(string cpf)
        {
            
            _context.Beneficiarios.Add(new BeneficiarioModel
            {
                NomeCompleto = "Beneficiario Existente",
                Cpf = cpf,
                PlanoId = 1,
                DataNascimento = new DateTime(01/01/2000),
                Status = Status.ATIVO
            });
            await _context.SaveChangesAsync();

            
            return CriarService();
        }

        // Cria servi�o com lista de benefici�rios 
        private async Task<BeneficiarioService> CriarServiceComListaDeBeneficiarios()
        {
             
            _context.Beneficiarios.AddRange(new List<BeneficiarioModel>
    {
            new BeneficiarioModel { NomeCompleto = "Lucas de Jesus Marinho", Cpf = "11111111111", PlanoId = 1, Status = Status.ATIVO, DataNascimento= new DateTime(01/01/2000)},
            new BeneficiarioModel { NomeCompleto = "Ana Alice de Jesus da Silva", Cpf = "22222222222", PlanoId = 2, Status = Status.INATIVO, DataNascimento= new DateTime(02/02/2000) },
            new BeneficiarioModel { NomeCompleto = "Maria de Jesus da Silva", Cpf = "33333333333", PlanoId = 1, Status = Status.ATIVO, DataNascimento= new DateTime (03/03/2000) }
            });
            await _context.SaveChangesAsync();

            
            return CriarService();
        }

        // Cria servi�o com benefici�rio ativo (para testes de atualiza��o de status)
        private async Task<BeneficiarioService> CriarServiceComBeneficiarioAtivo()
        {
            
            _context.Beneficiarios.Add(new BeneficiarioModel
            {
                NomeCompleto = "Lucas Ativo",
                Cpf = "55555555555",
                PlanoId = 1,
                DataNascimento = new DateTime(04/04/2000),
                Status = Status.ATIVO
            });
            await _context.SaveChangesAsync();

            
            return CriarService();
        }

        // TESTES

        [Fact]
        public async Task CriarBeneficiario_CPFDuplicado_DeveRetornarErro409()
        {
            // Arrange: cria servi�o com benefici�rio existente e DTO com mesmo CPF
            var cpf = "12345678910";
            var service = await CriarServiceComBeneficiarioExistente(cpf);

            var dto = new BeneficiarioCriacaoDto
            {
                NomeCompleto = "Novo Beneficiario",
                Cpf = cpf,
                PlanoId = 1,
                DataNascimento = new DateTime(01/01/2000)
            };

            // Act: tenta criar benefici�rio duplicado
            var result = await service.CriarBeneficiario(dto);

            // Assert: deve retornar erro de duplicidade
            result.Status.Should().BeFalse();
            result.Error.Should().Be("ValidationError");
            result.Mensagem.Should().Be("CPF j� cadastrado");
        }

        [Fact]
        public async Task CriarBeneficiario_CPFInvalido_DeveRetornarErro400()
        {
            // Arrange: cria servi�o e DTO com CPF inv�lido
            var service = CriarService();
            var dto = new BeneficiarioCriacaoDto
            {
                NomeCompleto = "CPF Invalido",
                Cpf = "11111111111",
                PlanoId = 1,
                DataNascimento = new DateTime(01/01/2000)
            };

            // Act: tenta criar benefici�rio com CPF inv�lido
            var result = await service.CriarBeneficiario(dto);

            // Assert: deve retornar erro de valida��o
            result.Status.Should().BeFalse();
            result.Error.Should().Be("ValidationError");
            result.Mensagem.Should().Be("CPF inv�lido");
        }

        [Fact]
        public async Task CriarBeneficiario_PlanoInexistente_DeveRetornarErro404()
        {
            // Arrange: cria servi�o e DTO com plano inexistente
            var service = CriarService();
            var dto = new BeneficiarioCriacaoDto
            {
                NomeCompleto = "Plano Inexistente",
                Cpf = "98765432100",
                PlanoId = 999,
                DataNascimento = new DateTime(01/01/2000)
            };

            // Act: tenta criar benefici�rio com plano que n�o existe
            var result = await service.CriarBeneficiario(dto);

            // Assert: deve retornar erro de plano n�o encontrado
            result.Status.Should().BeFalse();
            result.Error.Should().Be("NotFound");
            result.Mensagem.Should().Be("Plano n�o encontrado");
        }

        [Fact]
        public async Task AtualizarStatus_Beneficiario_DeveAlterarParaInativo()
        {
            // Arrange: cria servi�o com benefici�rio ativo e DTO alterando status
            var service = await CriarServiceComBeneficiarioAtivo();
            var beneficiario = _context.Beneficiarios.First();

            var dtoEdicao = new BeneficiarioEdicaoDto
            {
                Id = beneficiario.Id,
                NomeCompleto = beneficiario.NomeCompleto,
                Cpf = beneficiario.Cpf,
                DataNascimento = beneficiario.DataNascimento,
                Status = Status.INATIVO
            };

            // Act: atualiza status do benefici�rio
            var result = await service.EditarBeneficiarios(dtoEdicao);

            // Assert: deve retornar status verdadeiro e benefici�rio com status atualizado
            result.Status.Should().BeTrue();
            result.Dados.Status.Should().Be(Status.INATIVO);
        }

        [Fact]
        public async Task ListarBeneficiarios_ComFiltros_DeveRetornarSomenteCorretos()
        {
            // Arrange: cria servi�o com lista de benefici�rios
            var service = await CriarServiceComListaDeBeneficiarios();

            // Act: lista todos os benefici�rios
            var todos = (await service.ListarBeneficiarios()).Dados;

            // Assert: filtra apenas os ativos do plano 1
            var filtrados = todos.Where(b => b.Status == Status.ATIVO && b.PlanoId == 1).ToList();
            filtrados.Should().OnlyContain(b => b.Status == Status.ATIVO && b.PlanoId == 1);
        }
    }
}
