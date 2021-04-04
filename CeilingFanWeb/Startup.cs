using CeilingFanWeb.Filters;
using Infra.Contexts;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services.FanService;
using System.IO;
using System.Linq;
using System;

namespace CeilingFanWeb
{
    public class Startup
    {
        private readonly string swaggerBasePath = "api";

        private readonly string environmentName;
        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var basePath = Directory.GetCurrentDirectory();
            var applicationPath = Directory.GetCurrentDirectory() + "/";

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("connectStrings.json", false, true)
                .AddJsonFile(applicationPath + $"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            environmentName = env.EnvironmentName;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddControllers();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //Add Cors
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyOrigin()
                       .AllowAnyHeader();
            }));

            //Add DbContext
            services.AddDbContext<FanDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:CeilingFanConnStr"],
                    //Implement resilient Entity Framework Core SQL connections
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }
                    );
            });

            //Using mvc pattern
            services
                .AddMvc(setup =>
                {
                    setup.Filters.Add(typeof(MidExceptionFilter));
                })
                .AddJsonOptions(c =>
                {
                    c.JsonSerializerOptions.WriteIndented = true;
                    c.JsonSerializerOptions.PropertyNamingPolicy = null;
                    c.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });


            //Adding Swagger documentation
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"Ceiling Fan API",
                    Version = "v1",
                    Description = "Ceiling Fan API for BlueCross practical test",
                    Contact = new OpenApiContact
                    {
                        Name = "Pedro Araujo Medeiros",
                        Email = "pedro.araujo.medeiros@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/pedroaraujomedeiros/")
                    }
                });

                setup.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //setup.SchemaFilter<IgnorePropertiesSchemaFilter>();
                setup.IgnoreObsoleteActions();
            });



            /* 
            => Dependency Injection
            - Transient
            - Scoped
            - Singleton
            */
            services.AddTransient<IUnitOfWork, UnitOfWork<FanDbContext>>();
            //services.AddSingleton<IConfiguration>(Configuration);

            //Adding Services for dependency injection
            services.AddTransient<FanService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerBasePath + "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{swaggerBasePath}/swagger/v1/swagger.json", "CeilingFanAPI v1");
                c.RoutePrefix = $"{swaggerBasePath}/swagger";
                c.EnableValidator();

            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller}/{action=Index}/{id?}",
                //    defaults: new { controller = "Home", action = "Index" }
                //    );
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
