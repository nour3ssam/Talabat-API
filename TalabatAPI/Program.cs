using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using TalabatAPI.Controllers;
using TalabatAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.ServiceInterfaces;
using Talabat.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Talabat.Core;

namespace TalabatAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();






            builder.Services.AddDbContext<StoreContext>(option=>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            builder.Services.AddDbContext<AppIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(option=>
            {
                var connect = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connect);
            });





            // builder.Services.AddScoped<IGenericRepo<Product>, GenericRepo<Product>>();
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            builder.Services.AddScoped(typeof(IBasketRepo), typeof(BasketRepo));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddSingleton<IResponseCacheService, ResponceCacheService>();





            // builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles())); // Not Valid
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ChatGPT



            #region Identity

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };

                });

            builder.Services.AddScoped<ITokenService,TokenService>();

            # endregion


            var app = builder.Build(); 

            #region Ubdate Database
            using var scope = app.Services.CreateScope();// Group Of Services LifeTime Scoped
            var service = scope.ServiceProvider; // Services It Self
            var loggerFactory = service.GetRequiredService<ILoggerFactory>(); 

            try
            {
                var dbcontext = service.GetRequiredService<StoreContext>();// Ask CLR to Create Object From DBContext Explicitly
                await dbcontext.Database.MigrateAsync();// update database
                var DbContextIdentity = service.GetRequiredService<AppIdentityDbContext>(); 
                await DbContextIdentity.Database.MigrateAsync();

                var mangerUser = service.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(mangerUser);
                await StoreContextSeed.Seedasync(dbcontext);
            }
            catch (Exception ex) { 
                var Logger = loggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, " Error Ouccured During the Migration ");
            }
            #endregion



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(); // for to see wwwroot
            app.UseHttpsRedirection();
           app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
