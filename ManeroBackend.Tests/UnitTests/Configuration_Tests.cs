using Microsoft.Extensions.Configuration;
using Xunit;

namespace ManeroBackend.Tests.UnitTests;

public class Configuration_Tests
{
    [Fact]
    public void Can_Read_Google_Authentication_Settings()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") 
            .Build();

        // Act
        var clientId = configuration["Authentication:Google:ClientId"];
        var clientSecret = configuration["Authentication:Google:ClientSecret"];

        // Assert
        Assert.NotNull(clientId);
        Assert.NotNull(clientSecret);
    }
}
