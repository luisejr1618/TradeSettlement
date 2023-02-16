// Unused usings removed
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Service;

namespace TodoApi
{
    public class StartupJavaScript
    {
        public StartupJavaScript(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<ISettlementService, SettlementService>();
            services.AddSingleton<ITradesService, TradesService>();
            services.AddDbContext<TradesContext>(opt =>
               opt.UseInMemoryDatabase("Trades"), ServiceLifetime.Singleton);
            services.AddSwaggerGen();
            services.AddControllers();
        }

        #region snippet_configure
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

    }

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
        #endregion
    }
}
