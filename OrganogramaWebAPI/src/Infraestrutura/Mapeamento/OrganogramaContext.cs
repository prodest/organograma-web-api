﻿using Organograma.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;


namespace Organograma.Infraestrutura.Mapeamento
{
    public partial class OrganogramaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=10.32.254.137;Database=Organograma;User Id=Apl_Organograma;Password=Um9gJ0;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EsferaOrganizacao>(entity =>
            {
                entity.HasIndex(e => e.Descricao)
                    .HasName("UK_EsferaOrganizacaoDescricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasIndex(e => e.CodigoIbge)
                    .HasName("UQ__codigoIbge")
                    .IsUnique();

                entity.HasIndex(e => new { e.Nome, e.Uf })
                    .HasName("UQ_nome_uf")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodigoIbge).HasColumnName("codigoIbge");

                entity.Property(e => e.FimVigencia)
                    .HasColumnName("fimVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.InicioVigencia)
                    .HasColumnName("inicioVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ObservacaoFimVigencia)
                    .HasColumnName("observacaoFimVigencia")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Uf)
                    .IsRequired()
                    .HasColumnName("uf")
                    .HasColumnType("varchar(2)");
            });

            modelBuilder.Entity<TipoOrganizacao>(entity =>
            {
                entity.HasIndex(e => e.Descricao)
                    .HasName("UK_TipoUnidadeDescricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.FimVigencia)
                    .HasColumnName("fimVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.InicioVigencia)
                    .IsRequired()
                    .HasColumnName("inicioVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.ObservacaoFimVigencia)
                    .HasColumnName("observacaoFimVigencia")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<TipoUnidade>(entity =>
            {
                entity.HasIndex(e => e.Descricao)
                    .HasName("UK_TipoUnidadeDescricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.FimVigencia)
                    .HasColumnName("fimVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.InicioVigencia)
                    .IsRequired()
                    .HasColumnName("inicioVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.ObservacaoFimVigencia)
                    .HasColumnName("observacaoFimVigencia")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Poder>(entity =>
            {
                entity.HasIndex(e => e.Descricao)
                    .HasName("UK_EsferaOrganizacaoDescricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Poder>(entity =>
            {
                entity.HasIndex(e => e.Descricao)
                    .HasName("UQ__PoderDescricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(100)");
            });


        }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<TipoOrganizacao> TipoOrganizacao { get; set; }
        public virtual DbSet<TipoUnidade> TipoUnidade { get; set; }
        public virtual DbSet<Poder> Poder { get; set; }

    }
}