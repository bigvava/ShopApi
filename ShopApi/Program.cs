using Microsoft.EntityFrameworkCore;
using ShopApi.Entities;
using ShopApi.Repositories;
using ShopApi.Services;

namespace ShopApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                //?? ?????? ?????????? ????????????? ??????? ???????? ??????????.?????? ?? ????????? ?????????
                var redisConnectionString = builder.Configuration["RedisCache:ConnectionString"];
                options.Configuration = redisConnectionString;
            });

            builder.Services.AddDbContext<ShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(Program));
            //builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.Run();
        }
    }
}
