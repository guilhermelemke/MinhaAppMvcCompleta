using System;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Context
{
	public class MeuDbContext : DbContext
	{
        public MeuDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Produto>? Produtos { get; set; }
		public DbSet<Endereco>? Enderecos { get; set; }
		public DbSet<Fornecedor>? Fornecedores { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			//Não permitir mapeamento com NVARCHAR(MAX)
			foreach (var property in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetProperties()
				.Where(p => p.ClrType == typeof(string))))
				property.SetColumnType("varchar(100)");

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

			// Prevents CASCADE delete, if any Entity has a child
			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

			base.OnModelCreating(modelBuilder);
        }
	}
}

