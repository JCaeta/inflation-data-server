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

        //string origin = Environment.GetEnvironmentVariable("ORIGIN") ?? "http://localhost:3000";
        //Console.WriteLine("origin: " + origin);
        services.AddCors(options =>
        {

            options.AddPolicy(name: "_myAllowSpecificOrigins", builder =>
            {

                builder.WithOrigins("https://inflation-data-e2924.web.app")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
        });
    }

    public static void Configure(WebApplication app)
    {
        // Configure the HTTP request pipeline.

        //app.UseCors("_myAllowSpecificOrigins");


        //string origin = Environment.GetEnvironmentVariable("ORIGIN") ?? "http://localhost:3000";
        //app.UseCors(
        //  options => options.WithOrigins(origin).AllowAnyMethod().AllowAnyHeader()
        //      );
        app.UseCors("_myAllowSpecificOrigins");

        //app.UseMvc();
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