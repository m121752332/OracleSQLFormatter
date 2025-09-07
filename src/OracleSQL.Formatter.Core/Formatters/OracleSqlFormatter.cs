using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using OracleSQL.Formatter.Common.Enums;
using OracleSQL.Formatter.Common.Extensions;
using OracleSQL.Formatter.Core.Models;

namespace OracleSQL.Formatter.Core.Formatters;

public class OracleSqlFormatter : ISqlFormatter
{
    // Expanded keyword list
    private static readonly string[] Keywords = new[]
    {
        "SELECT","FROM","WHERE","ORDER BY","GROUP BY","INNER JOIN","LEFT JOIN","RIGHT JOIN","FULL JOIN","CROSS JOIN",
        "JOIN","ON","AND","OR","INSERT","INTO","VALUES","UPDATE","SET","DELETE","CREATE","TABLE","ALTER","DROP",
        "UNION","UNION ALL","HAVING","DISTINCT","AS","IN","IS","NULL","NOT","LIKE","BETWEEN","EXISTS","CASE","WHEN","THEN","END","ELSE","LIMIT","OFFSET","FETCH","FIRST","ONLY"
    };

    private static readonly Regex MultiSpace = new(@"\n|\r|\t|\s+", RegexOptions.Compiled);

    public FormatterResult Format(string sql, FormatterOptions options)
    {
        sql ??= string.Empty;
        var original = sql;

        // remove leading/trailing
        sql = sql.Trim();

        if (options.RemoveExistingNewLines)
        {
            sql = MultiSpace.Replace(sql, " ").Trim();
        }

        // Normalize commas spacing
        sql = Regex.Replace(sql, @",\s*", ", ");

        // Ensure single space around operators
        sql = Regex.Replace(sql, @"\s*([=<>+-])\s*", " $1 ");

        // Insert newlines before major clauses (preserve order by using longer first)
        // NOTE: omit the generic single-word 'JOIN' here to avoid double-inserting newlines inside 'INNER JOIN' etc.
        var clauses = new[] { "UNION ALL", "UNION", "ORDER BY", "GROUP BY", "INNER JOIN", "LEFT JOIN", "RIGHT JOIN", "FULL JOIN", "CROSS JOIN", "WHERE", "FROM", "SELECT", "HAVING" };
        foreach (var kw in clauses.OrderByDescending(k => k.Length))
        {
            sql = Regex.Replace(sql, $"\\b{Regex.Escape(kw)}\\b", m => "\n" + m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);
        }

        // Put AND/OR on new line when in WHERE clause
        sql = Regex.Replace(sql, @"\b(AND|OR)\b", m => "\n    " + m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

        // Put ON on new line indented
        sql = Regex.Replace(sql, @"\bON\b", m => "\n    " + m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

        // Apply keyword casing
        foreach (var kw in Keywords.OrderByDescending(k => k.Length))
        {
            sql = Regex.Replace(sql, Regex.Escape(kw), m => ApplyCase(m.Value, options.KeywordCase), RegexOptions.IgnoreCase);
        }

        // Handle SELECT list stacking with alignment
        if (options.ListStyle.Equals("Stacked", System.StringComparison.OrdinalIgnoreCase))
        {
            sql = Regex.Replace(sql, @"SELECT\s+((?:DISTINCT\s+)?)\s*(.*?)\s+FROM", m =>
            {
                var distinct = m.Groups[1].Value; // may be empty or "DISTINCT "
                var list = m.Groups[2].Value;
                var parts = SplitTopLevel(list, ',').Select(p => p.Trim()).ToArray();

                var anchor = $"SELECT {distinct}"; // e.g. "SELECT DISTINCT " or "SELECT "
                var indent = new string(' ', anchor.Length);

                var sb = new StringBuilder();
                sb.Append(anchor);

                for (int i = 0; i < parts.Length; i++)
                {
                    var item = FormatFunctionCalls(parts[i], indent);
                    if (i == 0)
                    {
                        sb.Append(item);
                    }
                    else
                    {
                        if (options.Align.Equals("Align right", System.StringComparison.OrdinalIgnoreCase))
                            sb.Append("\n" + indent + item);
                        else
                            sb.Append("\n    " + item); // default 4 spaces left-align
                    }
                    if (i < parts.Length - 1) sb.Append(",");
                }
                sb.Append("\nFROM");
                return sb.ToString();
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        // Handle ORDER BY stacking with alignment
        if (options.ListStyle.Equals("Stacked", System.StringComparison.OrdinalIgnoreCase))
        {
            sql = Regex.Replace(sql, @"ORDER BY\s+(.*?)($|\n)", m =>
            {
                var list = m.Groups[1].Value.Trim();
                var parts = SplitTopLevel(list, ',').Select(p => p.Trim()).ToArray();
                var sb = new StringBuilder();
                sb.Append("ORDER BY ");
                var indent = new string(' ', "ORDER BY ".Length);

                for (int i = 0; i < parts.Length; i++)
                {
                    var item = FormatFunctionCalls(parts[i], indent);
                    if (i == 0) sb.Append(item);
                    else
                    {
                        if (options.Align.Equals("Align right", System.StringComparison.OrdinalIgnoreCase))
                            sb.Append("\n" + indent + item);
                        else
                            sb.Append("\n    " + item);
                    }
                    if (i < parts.Length - 1) sb.Append(",");
                }
                if (m.Groups.Count > 1 && m.Groups[2].Success) sb.Append(m.Groups[2].Value);
                return sb.ToString();
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        // Normalize multiple newlines to single
        sql = Regex.Replace(sql, @"(\n\s*){2,}", "\n");

        // Trim lines but keep leading indentation (important for alignment)
        var lines = sql.Split('\n').Select(l => l.TrimEnd()).ToList();

        // Anchor-based indentation for subqueries like NOT EXISTS (SELECT ...)
        bool rightAlign = options.Align.Equals("Align right", System.StringComparison.OrdinalIgnoreCase);
        int anchorSpaces = 0;
        bool anchorActive = false;

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            // Detect previous line having (SELECT
            if (rightAlign && !anchorActive && i > 0)
            {
                var prev = lines[i - 1];
                int selIdx = prev.IndexOf("(SELECT", System.StringComparison.OrdinalIgnoreCase);
                if (selIdx >= 0)
                {
                    anchorSpaces = selIdx + 1 + "SELECT ".Length; // align after "SELECT " inside parentheses
                    anchorActive = true;
                }
            }

            // Apply anchor indentation for FROM/WHERE/AND/OR within the subquery
            if (rightAlign && anchorActive)
            {
                if (line.StartsWith("FROM ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("WHERE ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("AND ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("OR ", System.StringComparison.OrdinalIgnoreCase))
                {
                    line = new string(' ', anchorSpaces) + line;
                }
            }
            else
            {
                // Top-level indentation: indent FROM/WHERE by 2 spaces for readability
                if (line.StartsWith("FROM ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("WHERE ", System.StringComparison.OrdinalIgnoreCase))
                {
                    line = "  " + line;
                }

                // 4-space indent for ON/AND/OR on top level
                if (line.StartsWith("ON ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("AND ", System.StringComparison.OrdinalIgnoreCase)
                    || line.StartsWith("OR ", System.StringComparison.OrdinalIgnoreCase))
                {
                    line = "    " + line;
                }
            }

            // Deactivate anchor when encountering a closing parenthesis on a line
            if (anchorActive && line.Contains(')'))
            {
                anchorActive = false;
            }

            lines[i] = line;
        }

        var formatted = string.Join('\n', lines).Trim();

        // Trim trailing spaces on each line
        formatted = string.Join('\n', formatted.Split('\n').Select(l => l.TrimEnd()));

        return new FormatterResult
        {
            Original = original,
            Formatted = formatted
        };
    }

    // Insert line break after opening parenthesis for known functions when stacked; keep simple for TO_DATE
    private static string FormatFunctionCalls(string item, string indent)
    {
        var result = item;
        var match = Regex.Match(result, @"\bTO_DATE\s*\(", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            // insert a newline after the first '('
            int idx = result.IndexOf('(');
            if (idx >= 0 && idx < result.Length - 1)
            {
                result = result.Substring(0, idx + 1) + "\n" + indent + result.Substring(idx + 1);
            }
        }
        return result;
    }

    // Split at top-level commas (not inside parentheses)
    private static IEnumerable<string> SplitTopLevel(string input, char separator)
    {
        var parts = new List<string>();
        if (string.IsNullOrEmpty(input)) return parts;
        int depth = 0;
        var sb = new StringBuilder();
        foreach (var ch in input)
        {
            if (ch == '(') depth++;
            if (ch == ')') depth = Math.Max(0, depth - 1);
            if (ch == separator && depth == 0)
            {
                parts.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(ch);
            }
        }
        parts.Add(sb.ToString());
        return parts;
    }

    private static string ApplyCase(string input, CaseOption option)
    {
        return input.ToCase(option);
    }
}
