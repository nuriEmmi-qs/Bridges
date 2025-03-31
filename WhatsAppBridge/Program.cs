using WhatsAppBridge.Extensions;

internal class Program {
    private static void Main() {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();
        builder.Services.AddWhatsAppBridgeServices();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseWhatsAppBridgeMiddlewares();

        app.Run();
    }
}
