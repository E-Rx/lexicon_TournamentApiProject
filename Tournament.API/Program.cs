using Microsoft.EntityFrameworkCore;
using Tournament.API.Extensions;
using Domain.Contracts.Repositories;
using Tournament.Data.Repositories;
using Tournament.Data.Data;
using Service.Contracts;
using Tournament.Services.Services;

namespace Tournament.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TournamentAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentAPIContext") ?? throw new InvalidOperationException("Connection string 'TournamentAPIContext' not found.")));


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITournamentDetailsRepository, TournamentDetailsRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();

            // Add Scoped Services
            builder.Services.AddScoped<ITournamentDetailsService, TournamentDetailsService>();
            builder.Services.AddScoped<IGameService, GameService>();

            // Add Lazy Services
            builder.Services.AddLazyServices();


            builder.Services.AddScoped<IServiceManager, ServiceManager>();


            // Add services to the container
            builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(TournamentMappings));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();


            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TournamentAPIContext>();
                await SeedData.InitAsync(context);
            }

            app.Run();
        }
    }
}
