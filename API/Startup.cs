using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using API.Repositories;
using API.Repositories.PlayerRepo;
using API.Repositories.GameRepo;
using API.Services.PlayerService;
using API.Services.GameService;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);;
            // services.AddCors(options =>
            // {
            //     options.AddPolicy(MyAllowSpecificOrigins,
            //         builder =>
            //         {
            //             builder.WithOrigins().AllowAnyHeader().AllowAnyOrigin();
            //         });
            // });



            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IGameService, GameService>();
            services.AddDbContext<AppDataContext>( options =>
                // options.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;",
                // b => b.MigrationsAssembly("API")));
            ///*original ->*/  options.UseSqlite("Data Source=tcp:mballers.database.windows.net,1433;Initial Catalog=mball;Persist Security Info=False;User ID=orriaxels;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", b => b.MigrationsAssembly("API")));
              options.UseSqlServer("Server=tcp:mballers.database.windows.net,1433;Initial Catalog=mballers;Persist Security Info=False;User ID=orriaxels;Password=Y81XVa8UvEt1YUPoLEl4%ucGDh1^q%zX4hRlzJ5e^oDchQ$5YU@TZ#egJkRzJ9DL9cr2S2fxln5!pn&d$unz;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", 
                                    b => b.MigrationsAssembly("API")));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
			{
				app.UseHsts();
			}

            app.UseSwagger();            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // c.RoutePrefix = string.Empty;
            });



            //app.UseCors(MyAllowSpecificOrigins);        

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
