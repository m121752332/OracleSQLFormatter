using OracleSQL.Formatter.Core.Models;

namespace OracleSQL.Formatter.Core.Formatters;

public interface ISqlFormatter
{
    FormatterResult Format(string sql, FormatterOptions options);
}
