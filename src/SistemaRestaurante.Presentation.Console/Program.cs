using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaRestaurante.Application.Interfaces;
using SistemaRestaurante.Application.Services;
using SistemaRestaurante.Domain.Entities;
using SistemaRestaurante.Domain.Interfaces;
using SistemaRestaurante.Domain.Services;
using SistemaRestaurante.Infrastructure.Context;
using SistemaRestaurante.Infrastructure.Repositories;

// ==================================================================
// Sistema Nhac - Controle de Estoque para restaurates
// Author: Roberto Escher
// ==================================================================


namespace SistemaRestaurante.Presentation.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configura o appsettings.json
            // var builder = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Procura na pasta onde o executável foi gerado
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // Configuração do contêiner de Injeção de Dependência (DI)
            var services = new ServiceCollection();
            
            services.AddDbContext<RestauranteDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Injeção de Repositórios e Serviços (DDD)
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<ISugestaoCompraDomainService, SugestaoCompraDomainService>();
            services.AddScoped<ISugestaoCompraAppService, SugestaoCompraAppService>();

            var serviceProvider = services.BuildServiceProvider();

            // Inicializa o banco de dados (Criação e Carga Inicial)
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RestauranteDbContext>();
                await context.Database.EnsureCreatedAsync();
                await SeedDatabaseAsync(context);

                // Executa o Serviço de Aplicação
                var appService = scope.ServiceProvider.GetRequiredService<ISugestaoCompraAppService>();
                var sugestoes = await appService.ObterSugestoesDaSemanaAsync();

                // Exibição do relatório
                Console.WriteLine("\n=== LISTA DE COMPRAS SUGERIDAS ===");
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("{0,-15} | {1,-12} | {2,-15} | {3,-15}", "Produto", "Estoque Atual", "Consumo Estimado", "A Comprar (+20%)");
                Console.WriteLine("-----------------------------------------------------------------------------");

                foreach (var item in sugestoes)
                {
                    Console.WriteLine("{0,-15} | {1,-12} | {2,-15} | {3,-15}", 
                        item.NomeProduto, 
                        $"{item.EstoqueAtual}{item.UnidadeMedida}", 
                        $"{item.ConsumoEstimado}{item.UnidadeMedida}", 
                        $"{item.QuantidadeParaComprar} {item.UnidadeMedida}");
                }
                Console.WriteLine("-----------------------------------------------------------------------------");
            }
        }
        private static async Task SeedDatabaseAsync(RestauranteDbContext context)
        {
            // Verifica se já existem produtos para não duplicar dados
            if (await context.Produtos.AnyAsync()) return;

            // 1. Criar os produtos passando ID = 0 (deixando o SQL Server gerar o ID real)
            var arroz = new Produto(0, "Arroz", 5000, "g", 10000);
            var feijao = new Produto(0, "Feijão", 12000, "g", 5000);
            var carne = new Produto(0, "Carne Bovina", 3000, "g", 8000);

            await context.Produtos.AddRangeAsync(arroz, feijao, carne);
            await context.SaveChangesAsync(); // Salva para o banco gerar os IDs reais de arroz, feijao e carne

            // 2. Criar o Prato e usar as instâncias de produtos salvas para obter os IDs corretos
            var pratoPF = new Prato(0, "Prato Feito Tradicional");
            
            // Passamos o arroz.Id gerado automaticamente pelo banco
            pratoPF.AdicionarItemReceita(new ItemReceita(0, pratoPF.Id, arroz.Id, 200)); 
            pratoPF.AdicionarItemReceita(new ItemReceita(0, pratoPF.Id, feijao.Id, 150)); 
            pratoPF.AdicionarItemReceita(new ItemReceita(0, pratoPF.Id, carne.Id, 180)); 

            await context.Pratos.AddAsync(pratoPF);
            await context.SaveChangesAsync(); // Salva para o banco gerar o ID do prato

            // 3. Criar as vendas vinculando o ID do prato criado
            var vendas = new List<VendaPrato>
            {
                new VendaPrato(0, pratoPF.Id, 30, DateTime.Now.AddDays(-3)),
                new VendaPrato(0, pratoPF.Id, 15, DateTime.Now.AddDays(-1))
            };

            await context.Vendas.AddRangeAsync(vendas);
            await context.SaveChangesAsync();
        }
    }
}