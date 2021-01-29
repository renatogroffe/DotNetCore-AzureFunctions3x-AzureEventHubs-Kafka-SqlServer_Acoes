using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FunctionAppProcessarAcoes.Data;

[assembly: FunctionsStartup(typeof(FunctionAppProcessarAcoes.Startup))]
namespace FunctionAppProcessarAcoes
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<AcoesRepository>();
        }
    }
}