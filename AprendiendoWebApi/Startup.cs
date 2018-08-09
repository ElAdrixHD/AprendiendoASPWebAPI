using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AprendiendoWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AprendiendoWebApi
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
            services.AddDbContext<ApplicationDbContext>(option => option.UseInMemoryDatabase("paisDB"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(ConfigureJson);
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
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

            if (!context.Paises.Any())
            {
                context.Paises.AddRange(new List<Pais>()
                {
                    new Pais(){Nombre = "España", Provincias = new List<Provincia>(){
                            new Provincia(){Nombre = "Granada"}
                    } },
                    new Pais()
                    { Nombre = "Francia", Provincias = new List<Provincia>()
                        {
                                new Provincia(){Nombre = "Paris"},
                                new Provincia(){Nombre = "Niza"}
                        }
                    },
                    new Pais(){Nombre = "Alemania"}
                });

                context.SaveChanges();
            }
        }
    }
}
