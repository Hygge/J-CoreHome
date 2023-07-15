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

            // ����ڴ滺��
            builder.Services.AddMemoryCache();
            // ע��jwt
            builder.Services.AddScoped<JwtHelper>();

            // �������ݿ�
            builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("mysqlServer"), new MySqlServerVersion(new Version(5, 7)))
                .LogTo(Console.WriteLine, LogLevel.Debug));

            /*            builder.Services.AddDbContext<BlogDbContext>(options =>            
                            options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer") ??
                            throw new InvalidOperationException("Connection string 'MvcMovieContext' not found."))
                            .LogTo(Console.WriteLine, LogLevel.Debug));*/

            // ע��ɸѡ��
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<GlobalExceptionFilter>();
            });

            //ͳһ����ʱ���ʽ,���÷��ص�ʱ���������ݸ�ʽ
            builder.Services.AddMvc().AddJsonOptions((options) => {
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
            });

            //���ÿ���
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
                        
            // ��������
            app.UseCors();
            // app.UseCors("Policy1");
            
            app.UseHttpsRedirection();

            //ʹ��Ĭ���ļ������м��������StaticFileOptions��ʵ�ַ�����Ŀ��Ŀ¼�ļ�
            // MyStaticFiles������ӳ���ļ��У��ɷ����ļ������ļ�
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
                //�����ļ�ǰ׺
                    RequestPath = ""
            });

            app.UseAuthorization();


            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // ��Ŀ������ѯ�Ƿ�����û������ڲ���һ������Ա�û�
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