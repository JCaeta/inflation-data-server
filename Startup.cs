using Microsoft.AspNetCore.Mvc;

public class Startup
{
    //private static readonly string _MyCors = "";

    public static WebApplication InitializeApp(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IServiceCollection services = builder.Services;
        ConfigureServices(services);

        var port = Environment.GetEnvironmentVariable("PORT") ?? "5001";
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

        WebApplication app = builder.Build();
        Configure(app);

        return app;
    }

    public static IConfiguration Configuration { get; }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ASP .Net API template", Version = "v1" });
        });

        //var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "secretkey";
        //var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? "audience";
        //var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? "issuer";
        //var subject = Environment.GetEnvironmentVariable("SUBJECT") ?? "subject";

        //Console.WriteLine("secretKey: " + secretKey);
        //Console.WriteLine("audience: " + audience);
        //Console.WriteLine("issuer: " + issuer);
        //Console.WriteLine("subject: " + subject);

        //Configuration["JwtConfig:SecretKey"] = secretKey;
        //Configuration["JwtConfig:Audience"] = audience;
        //Configuration["JwtConfig:Issuer"] = issuer;
        //Configuration["JwtConfig:Subject"] = subject;
    }

    public static void Configure(WebApplication app)
    {
        string origin = Environment.GetEnvironmentVariable("ORIGIN") ?? "http://localhost:3000";

        // global cors policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin 
            .AllowCredentials());

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP .Net API template"));
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

    }
}