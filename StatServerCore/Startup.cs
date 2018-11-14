using System;
using System.IO;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StatServerCore.Model;
using StatServerCore.Model.Mongo;
using Swashbuckle.AspNetCore.Swagger;

namespace StatServerCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Contact = new Contact {Email = "gabdrakhmanov.br@gmail.com", Name = "Bulat", Url = "https://t.me/bulat_gab"},
                    Description = "My simple web-api .NET Core application",
                    License = new License {Name = "Apache License 2.0", Url = "https://github.com/bulat-gab/StatServer/blob/master/LICENSE"},
                    Version = "v1",
                    Title = "StatServer"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<DbSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<StatServerContext>()
                   .As<IStatServerContext>()
                   .SingleInstance();

            builder.RegisterType<ServersRepository>()
                   .As<IServersRepository>()
                   .SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.ColoredConsole()
                         .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log\\log-{Date}.txt"),
                                              outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}")
                         .CreateLogger();

            app.UseSwagger()
               .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}