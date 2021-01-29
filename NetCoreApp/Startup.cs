using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreApp.Business;
using NetCoreApp.Database;
using NetCoreApp.Hubs;

namespace NetCoreApp
{
  public class Startup
  {
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices( IServiceCollection services )
    {
      services.AddDbContext<DatabaseContext>( options => options
        .UseSqlite( Configuration.GetConnectionString( "DbConnection" ), builder => builder.MigrationsAssembly( "NetCoreApp.Database" ) ) )
        .AddUnitOfWork<DatabaseContext>();

      services.AddScoped<IUserService, UserService>();
      services.AddSingleton<IBroadcasterService, BroadcasterService>();

      services.AddSignalR( options =>
      {
        options.EnableDetailedErrors = true;
      } );

      services.AddCors( options =>
      {
        options.AddPolicy( "AllowCors", policy => policy
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
        );
      } );

      services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env )
    {
      if( env.IsDevelopment() )
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseCors( "AllowCors" );

      app.UseSignalR( routes =>
      {
        routes.MapHub<MainHub>( "/hub/main" );
      } );

      app.UseHttpsRedirection();
      app.UseMvc();

      using( IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope() )
      {
        DatabaseContext context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

        context.Database.Migrate();
      }
    }
  }
}
