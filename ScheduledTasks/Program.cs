using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.API.DbContexts;



var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot config = builder.Build();
string connectionString = config.GetConnectionString("sqlConnection");

var contextOptions = new DbContextOptionsBuilder<BusinessContext>()
    .UseSqlServer(connectionString)
    .Options;

BusinessContext context = new BusinessContext(contextOptions);