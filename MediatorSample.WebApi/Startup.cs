
using FluentValidation;
using MediatorSample.WebApi.Infrastructure.Context;
using MediatorSample.WebApi.PipelineBehaviours;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

namespace MediatorSample.WebApi
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
            //SQL Connection 
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            //Oracle Connection 
            services.AddDbContext<ApplicationContext>(options =>
                options.UseOracle(
                    Configuration.GetConnectionString("OracleConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\MediatorSample.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MediatorSample.WebApi",
                });

            });
            #endregion
          

            services.AddScoped<IApplicationContext>(provider => provider.GetService<ApplicationContext>());

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();

            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediatorSample.WebApi");
            });
            #endregion


            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseExceptionHandler(
            //     options =>
            //     {
            //         options.Run(async context =>
            //         {
            //             context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //             context.Response.ContentType = "text/html";
            //             var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
            //             if (null != exceptionObject)
            //             {
            //                 var errorMessage = $"{exceptionObject.Error.Message}";
            //                 await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
            //             }
            //         });
            //     });

         
        }
    }
}
