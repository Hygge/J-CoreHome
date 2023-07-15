using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Db
{
    /// <summary>
    /// ef core 配置
    /// </summary>
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }


        public DbSet<Article> Articles { get; set; } = default!; 
        public DbSet<Category> Categorys { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<FriendUrl> FriendUrls { get; set; } = default!;
        public DbSet<Setting> Settings { get; set; } = default!;
        public DbSet<FileInfom> FileInfos { get; set; } = default!;
        public DbSet<Blogsettings> Blogsettingss { get; set; } = default!;

    }
}
