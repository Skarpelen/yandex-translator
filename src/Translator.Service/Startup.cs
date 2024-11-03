using Microsoft.OpenApi.Models;

namespace Translator.Service
{
    using Translator.Service.Cache;
    using Translator.Service.gRPC;
    using Translator.Service.Services;
    using Translator.Service.Structs;
    using Translator.Shared.Interfaces;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGrpc();

            services.Configure<YandexConfiguration>(Configuration.GetSection("YandexConfiguration"));
            services.AddSingleton<ITranslationService, YandexTranslationService>();

            services.AddSingleton<TokenService>();

            services.AddSingleton<ICacheService, RuntimeCacheService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Translator.Service",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Translator.Service API V1");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<TranslatorService>();
            });
        }
    }
}