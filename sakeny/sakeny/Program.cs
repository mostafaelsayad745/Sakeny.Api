using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Services;
using Serilog;

namespace sakeny
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().
                MinimumLevel.Debug().
                WriteTo.Console().
                WriteTo.File("logs\\sakeny.txt", rollingInterval: RollingInterval.Day).
                CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            options.ReturnHttpNotAcceptable = true
            ).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            builder.Services.AddDbContext<HOUSE_RENT_DBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:SakenyDbConnectionString"]);
            });


            builder.Services.AddScoped<IUserInfoRepository, UserInfoRepositorycs>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}