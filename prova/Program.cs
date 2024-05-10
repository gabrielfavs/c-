using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace FolhaPagamentoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public decimal ValorHora { get; set; } // Adicionado para calcular o salário bruto
    }

    public class FolhaPagamento
    {
        public int Id { get; set; }
        public Funcionario Funcionario { get; set; }
        public int HorasTrabalhadas { get; set; } // Adicionado para calcular o salário bruto
        public decimal SalarioBruto { get; set; } // Adicionado para armazenar o salário bruto
        public decimal ImpostoRenda { get; set; } // Adicionado para armazenar o imposto de renda
        public decimal INSS { get; set; } // Adicionado para armazenar o INSS
        public decimal FGTS { get; set; } // Adicionado para armazenar o FGTS
        public decimal SalarioLiquido { get; set; } // Adicionado para armazenar o salário líquido
        public int Mes { get; set; }
        public int Ano { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<FolhaPagamento> FolhasPagamento { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Dupla_X_Y.db");
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddRouting();
            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class FolhaPagamentoController : Controller
    {
        private readonly AppDbContext _context;

        public FolhaPagamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/api/folha/cadastrar")]
        public async Task<IActionResult> CadastrarFolhaPagamento([FromBody] FolhaPagamento folhaPagamento)
        {
           
            decimal salarioBruto = folhaPagamento.HorasTrabalhadas * folhaPagamento.Funcionario.ValorHora;

           
            decimal impostoRenda = CalcularImpostoRenda(salarioBruto);

           
            decimal inss = CalcularINSS(salarioBruto);

            
            decimal fgts = salarioBruto * 0.08m;

        
            decimal salarioLiquido = salarioBruto - impostoRenda - inss;

           
            folhaPagamento.SalarioBruto = salarioBruto;
            folhaPagamento.ImpostoRenda = impostoRenda;
            folhaPagamento.INSS = inss;
            folhaPagamento.FGTS = fgts;
            folhaPagamento.SalarioLiquido = salarioLiquido;

            _context.FolhasPagamento.Add(folhaPagamento);
            await _context.SaveChangesAsync();
            return Ok();
        }

     
        private decimal CalcularImpostoRenda(decimal salarioBruto)
        {
            if (salarioBruto <= 1903.98m)
            {
                return 0;
            }
            else if (salarioBruto <= 2826.65m)
            {
                return salarioBruto * 0.075m - 142.80m;
            }
            else if (salarioBruto <= 3751.05m)
            {
                return salarioBruto * 0.15m - 354.80m;
            }
            else if (salarioBruto <= 4664.68m)
            {
                return salarioBruto * 0.225m - 636.13m;
            }
            else
            {
                return salarioBruto * 0.275m - 869.36m;
            }
        }

        
        private decimal CalcularINSS(decimal salarioBruto)
        {
            if (salarioBruto <= 1693.72m)
            {
                return salarioBruto * 0.08m;
            }
            else if (salarioBruto <= 2822.90m)
            {
                return salarioBruto * 0.09m;
            }
            else if (salarioBruto <= 5645.80m)
            {
                return salarioBruto * 0.11m;
            }
            else
            {
                return 621.03m;
            }
        }
    }
}
