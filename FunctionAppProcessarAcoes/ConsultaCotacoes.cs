using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionAppProcessarAcoes.Data;

namespace FunctionAppProcessarAcoes
{
    public class ConsultaCotacoes
    {
        private readonly AcoesRepository _repository;

        public ConsultaCotacoes(AcoesRepository repository)
        {
            _repository = repository;
        }

        [FunctionName("Historico")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var dados = _repository.GetAll();

            log.LogInformation(
                $"Historico HTTP trigger: {dados.Count()} registro(s) encontrado(s)");

            return new OkObjectResult(dados);
        }
    }
}