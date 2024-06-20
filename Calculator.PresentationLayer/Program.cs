using Calculator.ApplicationLayer;
using Calculator.InfrastructureLayer;
using Calculator.PresentationLayer.Components;

namespace Calculator.PresentationLayer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.ConfigureApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices();
        
        builder.Services.AddLogging(cfg => cfg.AddConsole());

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        app.Run();
    }
}