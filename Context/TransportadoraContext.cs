using Microsoft.EntityFrameworkCore;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Context
{
    public class TransportadoraContext : DbContext
    {
        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Caminhao> Caminhoes { get; set; }
        public DbSet<AcaoNotaFiscal> AcoesNotaFiscal { get; set; }
        public DbSet<Fechamento> Fechamentos { get; set; }

        public TransportadoraContext(DbContextOptions<TransportadoraContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotaFiscal>()
                .HasMany(nf => nf.Acoes)
                .WithOne(an => an.NotaFiscal)
                .HasForeignKey(an => an.NotaFiscalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AcaoNotaFiscal>()
                .HasOne(an => an.Caminhao)
                .WithMany()
                .HasForeignKey(an => an.CaminhaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caminhao>()
                .Property(c => c.CustoCombustivel)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Caminhao>()
                .Property(c => c.CustoManutencao)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}