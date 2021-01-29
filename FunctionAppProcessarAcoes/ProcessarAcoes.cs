using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using FunctionAppProcessarAcoes.Models;
using FunctionAppProcessarAcoes.Validators;
using FunctionAppProcessarAcoes.Data;

namespace FunctionAppProcessarAcoes
{
    public class ProcessarAcoes
    {
        private readonly AcoesRepository _repository;

        public ProcessarAcoes(AcoesRepository repository)
        {
            _repository = repository;
        }

        [FunctionName("ProcessarAcoesSql")]
        public void Run([KafkaTrigger(
            "BrokerKafka", "topic-acoes",
            ConsumerGroup = "processar_acoes-sql",
            Protocol = BrokerProtocol.SaslSsl,
            AuthenticationMode = BrokerAuthenticationMode.Plain,
            Username = "UserKafka",
            Password = "PasswordKafka"
            )]KafkaEventData<string> kafkaEvent,
            ILogger log)
        {
            string dados = kafkaEvent.Value.ToString();
            log.LogInformation($"ProcessarAcoesSql - Dados: {dados}");

            Acao acao = null;
            try
            {
                acao = JsonSerializer.Deserialize<Acao>(dados,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                log.LogError("ProcessarAcoesSql - Erro durante a deserializacao!");
            }

            if (acao != null)
            {
                var validationResult = new AcaoValidator().Validate(acao);
                if (validationResult.IsValid)
                {
                    log.LogInformation($"ProcessarAcoesSql - Dados pos formatacao: {JsonSerializer.Serialize(acao)}");
                    _repository.Save(acao);
                    log.LogInformation("ProcessarAcoesSql - Acao registrada com sucesso!");
                }
                else
                {
                    log.LogError("ProcessarAcoesSql - Dados invalidos para a Acao");
                    foreach (var error in validationResult.Errors)
                        log.LogError($"ProcessarAcoesSql - {error.ErrorMessage}");
                }
            }
        }

    }
}