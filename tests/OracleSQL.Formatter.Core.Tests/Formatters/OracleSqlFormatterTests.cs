using System.Text.RegularExpressions;
using FluentAssertions;
using OracleSQL.Formatter.Core.Formatters;
using OracleSQL.Formatter.Core.Models;
using Xunit;

namespace OracleSQL.Formatter.Core.Tests.Formatters;

public class OracleSqlFormatterTests
{
    [Fact]
    public void Format_SelectQuery_ListStackedAndKeywordsUppercase()
    {
        var formatter = new OracleSqlFormatter();
        var options = new FormatterOptions
        {
            KeywordCase = Common.Enums.CaseOption.Uppercase,
            ListStyle = "Stacked",
            RemoveExistingNewLines = true
        };

        var input = "select emp.employee_id,emp.first_name,emp.last_name,dept.department_name from employees emp inner join departments dept on emp.department_id=dept.department_id where emp.salary>5000 and dept.location_id=1700 order by emp.last_name,emp.first_name";

        var result = formatter.Format(input, options);

        // SELECT and FROM present
        result.Formatted.Should().Contain("SELECT");
        result.Formatted.Should().Contain("FROM employees emp");

        // INNER JOIN may be broken across newline; accept any whitespace between INNER and JOIN
        Regex.IsMatch(result.Formatted, "INNER\\s+JOIN\\s+departments\\s+dept", RegexOptions.IgnoreCase).Should().BeTrue();

        // ON clause spacing normalized
        Regex.IsMatch(result.Formatted, "ON\\s+emp\\.department_id\\s*=\\s*dept\\.department_id", RegexOptions.IgnoreCase).Should().BeTrue();

        // WHERE and AND
        result.Formatted.Should().Contain("WHERE");
        Regex.IsMatch(result.Formatted, "AND\\s+dept\\.location_id\\s*=\\s*1700", RegexOptions.IgnoreCase).Should().BeTrue();

        // ORDER BY stacked
        Regex.IsMatch(result.Formatted, "ORDER\\s+BY\\s+emp\\.last_name", RegexOptions.IgnoreCase).Should().BeTrue();
    }
}
