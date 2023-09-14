using Betatalks.Testcontainers.Api.JsonConverters;
using Betatalks.Testcontainers.Core.Interfaces.Repositories;
using Betatalks.Testcontainers.Core.Interfaces.Services;
using Betatalks.Testcontainers.Core.Services;
using Betatalks.Testcontainers.Infrastructure.Database;
using Betatalks.Testcontainers.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Betatalks.Testcontainers.Api;

#pragma warning disable CA1506
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddScoped<IUserService, UserService>()
            .AddDbContext<TestcontainersContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            })
            .AddScoped<IUserRepository, UserRepository>()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
            options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });

        var app = builder.Build();

        app.UseCors("default");
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
        });
        app.UseHttpsRedirection();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<TestcontainersContext>();
            if (context == null)
            {
                throw new InvalidOperationException("The retrieved TestcontainersContext is null, meaning that it has not been defined as a Service");
            }
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }

        await app.RunAsync().ConfigureAwait(false);
    }
}
#pragma warning restore CA1506
