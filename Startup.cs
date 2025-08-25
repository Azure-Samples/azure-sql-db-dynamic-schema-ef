using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Azure.SQLDB.Samples.DynamicSchema;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddDbContext<ToDoContext>(options => {
            options.UseSqlServer(Environment.GetEnvironmentVariable("MSSQL"));
        });
            
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin", builder =>
                builder.WithOrigins("http://localhost:5500").AllowAnyMethod().AllowAnyHeader()
            );
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCors("AllowOrigin");

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
