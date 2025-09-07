using OracleSQL.Formatter.Core.Models;

namespace OracleSQL.Formatter.Core.Services;

public interface IFormatterService
{
    FormatterResult Format(string sql, FormatterOptions options);
}
