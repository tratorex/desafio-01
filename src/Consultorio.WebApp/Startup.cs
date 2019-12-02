
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Consultorio.Dados.Repositorios;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Marvin.Sdk.DependencyInjection;
using Marvin.Sdk.Mvc;
using Marvin.Sdk.NH.Configuration;
using Marvin.Sdk.RestFull;
using Marvin.Sdk.Search.Configuration;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace Consultorio.WebApp
{
    public class Startup
    {
        //Obtem versão do Assembly que será utilizado na documentação do Swagger
        private readonly string Versao = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        private readonly Info VersionSwagger = new Info()
        {
            Title = "Consultorio API RestFul",
            Version = "0.0.0",
            Description = "API do projeto Consultorio para integração RestFul"
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //define versão da documentação
            VersionSwagger.Version = this.Versao;
            
            services.AddMvc()
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.Formatting = Formatting.Indented;
                 }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddNHibernate(Configuration)
               .MappedWith(typeof(ConsultaRepository).Assembly)
               .AddSqlServer()
               ;

            services.AddDependencyInjection()
                .AddRepositories()
                .AddSearchers(Configuration)
                .AddValidators()
                ;

            services.AddTransient<OnExceptionFilterAttribute>();
            services.AddSingleton<RequestClient>();
            services.AddCors();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(this.VersionSwagger.Version, this.VersionSwagger);

                // Set the comments path for the Swagger JSON and UI.
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDataBaseLogger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "docs";
                    c.DocumentTitle = "Consultorio RestFul";
                    c.DisplayRequestDuration();
                    c.DocExpansion(DocExpansion.None);
                    c.SwaggerEndpoint($"/swagger/{this.VersionSwagger.Version}/swagger.json", $"API Consultorio RestFul Versão: {this.VersionSwagger.Version}");

                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "docs";
                    c.DocumentTitle = "Consultorio RestFul";
                    c.DisplayRequestDuration();
                    c.DocExpansion(DocExpansion.None);
                    c.SwaggerEndpoint($"../swagger/{this.VersionSwagger.Version}/swagger.json", $"Documento do MicroService Consultorio Versão {this.VersionSwagger.Version}");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //https://docs.microsoft.com/pt-br/aspnet/core/security/cors?view=aspnetcore-2.2
            app.UseCors(builder =>
                        builder
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //app.UseMvc();

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
