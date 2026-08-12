using Eghatha.Application.Features.AiAssistant;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.AiAssistant
{
    public class ReportQueryExecutor : IReportQueryExecutor
    {
        private const int MaxRows = 500;
        private const int CommandTimeoutSeconds = 10;

        private readonly string _connectionString;
        private readonly ILogger<ReportQueryExecutor> _logger;

        public ReportQueryExecutor(IConfiguration config, ILogger<ReportQueryExecutor> logger)
        {
            _connectionString = config.GetConnectionString("AiReader")
                ?? throw new InvalidOperationException(
                    "Missing 'AiReader' connection string. This must point at the ai_reader " +
                    "SQL login with SELECT-only access to the report schema.");
            _logger = logger;
        }
        public async Task<IReadOnlyList<Dictionary<string, object?>>> ExecuteAsync(string sql, CancellationToken ct)
        {
            if (!SqlGuard.IsSafe(sql, out var reason))
            {
                _logger.LogWarning("Rejected AI-generated SQL: {Reason}. SQL={Sql}", reason, sql);
                throw new InvalidOperationException($"Query rejected: {reason}");
            }

            // Hard cap regardless of what the AI wrote — defense-in-depth against
            // a query that would otherwise scan/return an unbounded result set.
            var aliased = EnsureAliased(sql);
            var wrapped = $"SELECT TOP {MaxRows} * FROM ({aliased}) AS q";
           
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(wrapped, conn)
            {
                CommandTimeout = CommandTimeoutSeconds,
                CommandType = CommandType.Text
            };

            try
            {
                await conn.OpenAsync(ct);
                await using var reader = await cmd.ExecuteReaderAsync(ct);

                var results = new List<Dictionary<string, object?>>();
                while (await reader.ReadAsync(ct))
                {
                    var row = new Dictionary<string, object?>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    results.Add(row);
                }

                return results;
            }
            catch (SqlException ex)
            {
                // Surface a generic message upward — don't leak raw SQL Server error text
                // (which can reveal schema details) back to the admin/AI.
                _logger.LogError(ex, "SQL execution failed. SQL={Sql}", wrapped);
                throw new InvalidOperationException(
                    "The query could not be executed. It may reference a column or " +
                    "table that doesn't exist in the report schema.");
            }
        }


        private static string EnsureAliased(string sql)
        {
            // Matches COUNT(...)/SUM(...)/AVG(...)/MIN(...)/MAX(...) not already followed by AS <name>
            return Regex.Replace(
                sql,
                @"\b(COUNT|SUM|AVG|MIN|MAX)\s*\([^)]*\)(?!\s+AS\s+\w+)",
                m => $"{m.Value} AS Col_{m.Groups[1].Value}",
                RegexOptions.IgnoreCase);
        }

    }
    
}
