using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using FunctionAppProcessarAcoes.Models;

namespace FunctionAppProcessarAcoes.Data
{
    public class AcoesRepository
    {
        private readonly string _strConexaoSql;
        private readonly string _prefixoCodReferenciaSql;

        public AcoesRepository()
        {
            _strConexaoSql =
                Environment.GetEnvironmentVariable("SqlServer_Connection");
            _prefixoCodReferenciaSql =
                Environment.GetEnvironmentVariable("SqlServer_PrefixoCodReferencia"); 
        }

        public void Save(Acao acao)
        {
            new SqlConnection(_strConexaoSql).Insert(
                new HistoricoAcao()
                {
                    Codigo = acao.Codigo,
                    CodReferencia = $"{_prefixoCodReferenciaSql}_{acao.Codigo}{DateTime.Now:yyyyMMddHHmmss.fff}",
                    DataReferencia = DateTime.Now,
                    Valor = acao.Valor,
                    CodCorretora = acao.CodCorretora,
                    NomeCorretora = acao.NomeCorretora
                });
        }

        public IEnumerable<HistoricoAcao> GetAll()
        {
            return new SqlConnection(_strConexaoSql).Query<HistoricoAcao>(
                "SELECT * FROM dbo.HistoricoAcoes ORDER BY Id Desc");
        }
    }
}