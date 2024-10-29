﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projeto_Transportadora_MVC.Context;

#nullable disable

namespace Projeto_Transportadora_MVC.Migrations
{
    [DbContext(typeof(TransportadoraContext))]
    partial class TransportadoraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.AcaoNotaFiscal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CaminhaoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataAgendada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataDaAcao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descriacao")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("NotaFiscalId")
                        .HasColumnType("int");

                    b.Property<int?>("StatusAgendamento")
                        .HasColumnType("int");

                    b.Property<int>("TipoAcao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CaminhaoId");

                    b.HasIndex("NotaFiscalId");

                    b.ToTable("AcoesNotaFiscal");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.Caminhao", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<decimal?>("CustoCombustivel")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CustoManutencao")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.HasKey("id");

                    b.ToTable("Caminhoes");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.Fechamento", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("DataDoFechamento")
                        .HasColumnType("datetime2");

                    b.Property<int>("NotaFiscalId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("NotaFiscalId");

                    b.ToTable("Fechamentos");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.NotaFiscal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Caminhaoid")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataDaEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataDoFaturamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("EnderecoFaturado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NomeCliente")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NumeroDaCarga")
                        .HasColumnType("int");

                    b.Property<int>("NumeroNotaFiscal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Caminhaoid");

                    b.ToTable("NotasFiscais");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.AcaoNotaFiscal", b =>
                {
                    b.HasOne("Projeto_Transportadora_MVC.Models.Caminhao", "Caminhao")
                        .WithMany()
                        .HasForeignKey("CaminhaoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Projeto_Transportadora_MVC.Models.NotaFiscal", "NotaFiscal")
                        .WithMany("Acoes")
                        .HasForeignKey("NotaFiscalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Caminhao");

                    b.Navigation("NotaFiscal");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.Fechamento", b =>
                {
                    b.HasOne("Projeto_Transportadora_MVC.Models.NotaFiscal", "NotaFiscal")
                        .WithMany()
                        .HasForeignKey("NotaFiscalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotaFiscal");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.NotaFiscal", b =>
                {
                    b.HasOne("Projeto_Transportadora_MVC.Models.Caminhao", null)
                        .WithMany("NotasFiscais")
                        .HasForeignKey("Caminhaoid");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.Caminhao", b =>
                {
                    b.Navigation("NotasFiscais");
                });

            modelBuilder.Entity("Projeto_Transportadora_MVC.Models.NotaFiscal", b =>
                {
                    b.Navigation("Acoes");
                });
#pragma warning restore 612, 618
        }
    }
}
