using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Examples.WebApi.Infrastructure.Extensions;
using Examples.WebApi.Infrastructure.Startup;

#pragma warning disable CA1822 // Member 'xxx' does not access instance data and can be marked as static
#pragma warning disable IDE0053 // Use expression body for lambda expressions.

namespace Examples.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.UseCustomOptions());

            // ----- Adds Infrastructure.Startup.
            services.AddCustomCorsPolicy("http://localhost:3000");
            services.AddCustokmAntiforgery("X-XSRF-TOKEN");
            services.AddCustomSwaggerGen();
            services.AddCustomFilters(Configuration);
            services.AddCustomLocalization();

            // ----- Sets URLs Lower Case.
            services.AddRouting(options => options.LowercaseUrls = true);

            return;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseCustomSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examples.WebApi v1"));

            }

            // ----- Adds Infrastructure.Startup.
            app.UseCustomRequestLocalization();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            // ----- Adds security headers.
            app.UseCustomHttpHeaderSecurity();

            app.UseEndpoints(endpoints =>
            {
                // Use CSRF middleware.
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
