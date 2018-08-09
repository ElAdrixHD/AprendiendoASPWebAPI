using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprendiendoWebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

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
            services.AddDbContext<ApplicationDbContext>(option => option.UseMySql(Configuration.GetConnectionString("DefaultConnection"), mysqlOptions =>
             {
                 mysqlOptions.ServerVersion(new Version(10, 2, 13), ServerType.MariaDb);
             }));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "localhost",
                     ValidAudience = "localhost",
                     IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["this is my custom Secret key for authnetication"])),
                     ClockSkew = TimeSpan.Zero
                 });

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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            if (!context.Paises.Any())
            {
                context.Paises.AddRange(new List<Pais>()
                {
                    new Pais(){Nombre = "España", Provincias = new List<Provincia>(){
                            new Provincia(){Nombre = "Granada"},
                            new Provincia(){Nombre = "Málaga"},
                            new Provincia(){Nombre = "Madrid"}
                    } },
                    new Pais(){ Nombre = "Francia", Provincias = new List<Provincia>()
                        {
                                new Provincia(){Nombre = "Paris"},
                                new Provincia(){Nombre = "Niza"}
                        }
                    },
                    new Pais(){Nombre = "Alemania", Provincias = new List<Provincia>()
                    {
                        new Provincia(){Nombre = "Berlín"}
                    }
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
