using Microsoft.Extensions.Options;

public class Program {
    private static void Main() {

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();
        builder.Services.AddAppServices(builder.Configuration);

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseApp(app.Services.GetRequiredService<IOptions<AppSettings>>().Value ,
            builder.Environment.EnvironmentName);

        app.Run();
    }
}
