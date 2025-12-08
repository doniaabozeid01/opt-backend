
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using optimum.data.Context;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.repository.Repositories;
using optimum.service.Authentication;
using optimum.service.Product;
using optimum.service.Schools;
using optimum.service.Supplier;
using optimum.service.SupplierOffer;
using optimum.service.SupplierRate;
using optimum.service.SupplierRequests;
using optimum.service.TextRequestsParser;
using System.Text;

namespace optimum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();







            builder.Services.AddDbContext<OptimumDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true; // ??? ?? ????? ??? ???
                options.Password.RequiredLength = 6;  // ??? ???? ?????? (????? 6)
                options.Password.RequireLowercase = true; // ??? ?? ????? ??? ??? ????
                options.Password.RequireUppercase = false; // ?? ????? ??? ??? ????
                options.Password.RequireNonAlphanumeric = false; // ?? ???? ????? ????
                options.Password.RequiredUniqueChars = 1; // ??? ?? ???? ???? ??? ????? ??? ???? ?????
            })
                .AddEntityFrameworkStores<OptimumDbContext>()
                .AddDefaultTokenProviders();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], // ??????? ????????? ?? config
                    ValidAudience = builder.Configuration["Jwt:Audience"], // ??????? ????????? ?? config
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });




            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<ISchoolService, SchoolService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddHttpClient<ITextRequestParserService, TextRequestParserService>();
            builder.Services.AddScoped<ISuppliersService, SuppliersService>();
            builder.Services.AddScoped<ISupplierRequestService, SupplierRequestsService>();
            builder.Services.AddScoped<IRatingSupplierService, RatingSupplierService>();
            builder.Services.AddScoped<ISupplierOfferService, SupplierOfferService>();



            builder.Services.AddControllers();




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin() // ?????? ??? ????
                           .AllowAnyHeader() // ?????? ??? ??????
                           .AllowAnyMethod()); // ?????? ??? ????? HTTP
            });




            var app = builder.Build();


            app.UseCors("AllowAll");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
