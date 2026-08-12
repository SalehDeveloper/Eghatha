using Eghatha.Application.Features.AiAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.AiAssistant
{
    public static class SqlGuard
    {
        private static readonly string[] BlockedKeywords =
   {
        "INSERT", "UPDATE", "DELETE", "DROP", "ALTER", "MERGE",
        "EXEC", "EXECUTE", "TRUNCATE", "CREATE", "GRANT", "REVOKE",
        "xp_", "sp_", "OPENROWSET", "OPENQUERY", "OPENDATASOURCE",
        "BULK", "sys.", "INTO", "DECLARE", "WAITFOR"
    };

        public static bool IsSafe(string sql, out string? reason)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                reason = "Empty query.";
                return false;
            }

            var trimmed = sql.Trim();

            // Strip a single trailing semicolon, but reject anything with a semicolon
            // in the middle — that would mean multiple statements.
            if (trimmed.EndsWith(';'))
                trimmed = trimmed[..^1];

            if (trimmed.Contains(';'))
            {
                reason = "Only a single statement is allowed.";
                return false;
            }

            // Reject comment markers outright — no legitimate reporting query needs them,
            // and they're a common injection/obfuscation vector.
            if (trimmed.Contains("--") || trimmed.Contains("/*"))
            {
                reason = "Comments are not allowed in the query.";
                return false;
            }

            if (!Regex.IsMatch(trimmed, @"^\s*SELECT\s", RegexOptions.IgnoreCase))
            {
                reason = "Only SELECT statements are allowed.";
                return false;
            }

            foreach (var kw in BlockedKeywords)
            {
                var pattern = kw.EndsWith('_')
                    ? $@"\b{Regex.Escape(kw)}"          // prefix form: xp_, sp_
                    : $@"\b{Regex.Escape(kw)}\b";        // whole-word form

                if (Regex.IsMatch(trimmed, pattern, RegexOptions.IgnoreCase))
                {
                    reason = $"Disallowed keyword: {kw}";
                    return false;
                }
            }

            // Every FROM / JOIN target must be one of the whitelisted report.* views.
            var refs = Regex.Matches(trimmed, @"\b(?:FROM|JOIN)\s+([A-Za-z0-9_\.\[\]]+)",
                                      RegexOptions.IgnoreCase)
                             .Select(m => m.Groups[1].Value.Trim('[', ']'));

            foreach (var r in refs)
            {
                if (!ReportSchema.AllowedViews.Any(v => v.Equals(r, StringComparison.OrdinalIgnoreCase)))
                {
                    reason = $"Table or view not permitted: {r}";
                    return false;
                }
            }

            reason = null;
            return true;
        }
    }
}
