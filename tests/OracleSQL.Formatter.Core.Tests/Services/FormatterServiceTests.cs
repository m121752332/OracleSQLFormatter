using FluentAssertions;
using OracleSQL.Formatter.Core.Services;
using OracleSQL.Formatter.Core.Models;
using Xunit;

namespace OracleSQL.Formatter.Core.Tests.Services;

public class FormatterServiceTests
{
    [Fact]
    public void Service_Uses_Formatter_To_Format()
    {
        var service = new FormatterService();
        var options = new FormatterOptions { RemoveExistingNewLines = true };
        var input = "select 1";
        var result = service.Format(input, options);
        result.Should().NotBeNull();
        result.Formatted.Should().Contain("SELECT");
    }
}
