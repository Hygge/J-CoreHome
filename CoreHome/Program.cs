using CoreHome.Config;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text;

namespace CoreHome
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 添加内存缓存
            builder.Services.AddMemoryCache();
            // 注入jwt
            builder.Services.AddScoped<JwtHelper>();

            // 配置数据库
            builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("mysqlServer"), new MySqlServerVersion(new Version(5, 7)))
                .LogTo(Console.WriteLine, LogLevel.Debug));

            /*            builder.Services.AddDbContext<BlogDbContext>(options =>            
                            options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer") ??
                            throw new InvalidOperationException("Connection string 'MvcMovieContext' not found."))
                            .LogTo(Console.WriteLine, LogLevel.Debug));*/

            // 注册筛选器
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            //统一返回时间格式,配置返回的时间类型数据格式
            builder.Services.AddMvc().AddJsonOptions((options) => {
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
            });

            //配置跨域
            builder.Services.AddEndpointsApiExplorer();
            string[] urls = new string[] { "https://www.zeng164outlook.online","http://localhost:3000" };
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                );
                /*options.AddPolicy("Policy1", policy =>
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                    );

                options.AddPolicy("AnotherPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });*/
            });
          

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
                        
            // 开启跨域
            app.UseCors();
            // app.UseCors("Policy1");
            
            app.UseHttpsRedirection();

            //使用默认文件访问中间件，设置StaticFileOptions来实现访问项目根目录文件
            // MyStaticFiles服务器映射文件夹，可访问文件夹下文件
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
                //访问文件前缀
                    RequestPath = ""
            });

            app.UseAuthorization();


            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // 项目启动查询是否存在用户不存在插入一条管理员用户
                var db = services.GetRequiredService<BlogDbContext>();
                long count = db.Users.Count();
                if (count == 0)
                {
                    User user = new User { Id = 1, Name = "Admin", Email = "3163367790@qq.com", Password = SecurityUtil.SHA256Encrypt("123456") };
                    db.Users.Add(user);
                    db.SaveChanges();
                }

            }

            app.Run();
        }

    }
}