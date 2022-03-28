using Deliver;
using DeployApp;
const string useSqlArg = "localSql";
const string onlySqlArg = "onlySql";

var shouldRunSql = (args.Contains(useSqlArg) || args.Contains(onlySqlArg));
var shouldRunApp = !args.Contains(onlySqlArg);

if (shouldRunSql)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{env}.json")
        .Build();

    Deploy.RunSqlScript(config.GetConnectionString("DefaultConnection"));
}

if (shouldRunApp)
{
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        }).Build().Run();
}