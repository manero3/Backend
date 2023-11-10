using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeroBackend.Tests.IntegrationTests
{
    public class DBTests
    {
        [Fact]
        public void ConnectionString_SqlServer_To_Azure() 
        {
            //Arrange
            var expected = "Server=tcp:manero-sql.database.windows.net,1433;Initial Catalog=manero-sql;Persist Security Info=False;User ID=SqlAdmin;Password=BytMig123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //Act

            var result = configuration.GetConnectionString("SqlServer");

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
