using GraphiQl;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHLStats.Api.Helpers;
using NHLStats.Api.Models;
using NHLStats.Core.Data;
using NHLStats.Data;
using NHLStats.Data.Repositories;


namespace NHLStats.Api
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
            //KESTRAL
            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true;
            //});

            // IIS
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddMvc(setupAction =>
            {
                setupAction.EnableEndpointRouting = false;
            });

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("https://localhost:5001", "http://localhost:3000", "http://127.0.0.1:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });


            services.AddHttpContextAccessor();
            services.AddSingleton<ContextServiceLocator>();
            services.AddDbContext<NHLStatsContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:NHLStatsDb"]));
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<ISkaterStatisticRepository, SkaterStatisticRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<NHLStatsQuery>();
            services.AddSingleton<NHLStatsMutation>();
            services.AddSingleton<PlayerType>();
            services.AddSingleton<PlayerInputType>();
            services.AddSingleton<SkaterStatisticType>();

            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<NHLStatsSchema>();

            services.AddGraphQL(o => { o.ExposeExceptions = false; }).AddGraphTypes(ServiceLifetime.Scoped);


            //var sp = services.BuildServiceProvider();
            //services.AddSingleton<ISchema>(new NHLStatsSchema(new FuncDependencyResolver(type => sp.GetService(type))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NHLStatsContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins("https://localhost:5001", "http://localhost:3000", "http://127.0.0.1:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                });

            }

            //app.UseGraphiQl();

            app.UseGraphQL<NHLStatsSchema>("/graphql");
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseMvc();
            db.EnsureSeedData();
        }
    }
}
