using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TranslationManagement.Dal;
using TranslationManagement.Bll.Services;
using TranslationManagement.Bll.Services.TranslationJobFileReader;

namespace TranslationManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
            });

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentCors",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                               .AllowAnyHeader()
                               .SetIsOriginAllowed(_ => true)
                               .AllowCredentials();
                    });

            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=TranslationAppDatabase.db"));

            services.AddScoped<TranslationJobService>();
            services.AddSingleton<TranslationJobFileReaderFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"));

            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseCors("DevelopmentCors");
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
