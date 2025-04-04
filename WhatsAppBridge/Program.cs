using Microsoft.Extensions.Options;

public class Program {
    private static void Main() {

        var builder = WebApplication.CreateBuilder();

        //builder.Configuration
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json", optional: false)
        //    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        //    .AddEnvironmentVariables();

        builder.Services.AddControllers();

        builder.Services.AddAppServices(builder.Configuration);

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseApp(app.Services.GetRequiredService<IOptions<ApiSettings>>().Value,
                   builder.Environment);

        app.Run();
    }
}
