using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MultiTenantTest.Infraestructura.Admin;
using MultiTenantTest.Infraestructura.Productos;
using MultiTenantTest.Infraestructura.Repository;
using MultiTenantTest.Servicios.Organization.Services;
using MultiTenantTest.Servicios.Organization.Services.Implementacion;
using MultiTenantTest.Servicios.Product;
using MultiTenantTest.Servicios.Product.Implementacion;
using MultiTenantTest.Servicios.User.Services;
using MultiTenantTest.Servicios.User.Services.Implementacion;
using MultiTenantTest.Servicios.Vinculation.Services;
using MultiTenantTest.Servicios.Vinculation.Services.Implementacion;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MultiTenantTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Mappers
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion Mappers
            #region Conexion BD
            services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Admin")));
            services.AddDbContext<ProductDbContext>();
            #endregion
            #region DI Services
            services.AddTransient<IVinculationServices, VinculationServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IOrganizationServices, OrganizationServices>();
            services.AddTransient<IProductServices, ProductServices>();
            #endregion
            #region DI Repository
            services.AddTransient<IVinculationRepository, VinculationRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            #endregion
            // Add services to the container.
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                                  Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                     ClockSkew = TimeSpan.Zero
                 });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app)
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();


        }

    }
}
