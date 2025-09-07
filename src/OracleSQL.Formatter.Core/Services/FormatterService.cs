using OracleSQL.Formatter.Core.Formatters;
using OracleSQL.Formatter.Core.Models;

namespace OracleSQL.Formatter.Core.Services;

public class FormatterService : IFormatterService
{
    private readonly ISqlFormatter _formatter;

    public FormatterService(ISqlFormatter? formatter = null)
    {
        _formatter = formatter ?? new OracleSqlFormatter();
    }

    public FormatterResult Format(string sql, FormatterOptions options)
    {
        return _formatter.Format(sql, options);
    }
}
