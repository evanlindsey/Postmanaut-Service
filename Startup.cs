using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PostmanautService.Hubs;

namespace PostmanautService
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
            services.AddCors();

            services.AddSignalR();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string clientHost;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                clientHost = "http://localhost:3000";
            }
            else
            {
                clientHost = Environment.GetEnvironmentVariable("CLIENT_HOST");
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins(clientHost)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Postmanaut Action Service");
                });

                endpoints.MapHub<ActionHub>("/ActionHub");

                endpoints.MapControllers();
            });
        }
    }
}
