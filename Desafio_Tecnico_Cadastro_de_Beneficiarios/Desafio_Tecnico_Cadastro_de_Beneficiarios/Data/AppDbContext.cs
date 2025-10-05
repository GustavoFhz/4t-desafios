using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Data
{
    public class AppDbContext : DbContext
    {   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<PlanoModel> Planos { get; set; } //Criação da tabela de Planos
    public DbSet<BeneficiarioModel> Beneficiarios { get; set; } //Criação da tabela de Beneficiarios

    }
}
