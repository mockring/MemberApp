using MemerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MemerApp.Data
{
    /// <summary>
    /// EF Core 的 DbContext，用來對應資料庫
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 建構子：允許 DI 注入 DbContextOptions
        /// </summary>
        /// <param name="options">DbContextOptions 由 Startup/Program 注入</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // --------------------------------------------------------------------
        //  DbSet 屬性：對應資料表
        // --------------------------------------------------------------------
        public DbSet<MemberModel> Members { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        // --------------------------------------------------------------------
        //  可選：若你不想把連線字串寫進 appsettings.json，直接在此設定
        // --------------------------------------------------------------------
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=MemerAppDb;Trusted_Connection=True;");
            }
        }*/

        // --------------------------------------------------------------------
        //  模型配置：可在此設定屬性映射、預設值、索引等
        // --------------------------------------------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. 會員表
            modelBuilder.Entity<MemberModel>(builder =>
            {
                // 若想自訂資料表名稱
                // builder.ToTable("Members");

                // 建立時間自動填入資料庫內部時間
                builder.Property(m => m.CreatedDate)
                       .HasDefaultValueSql("GETDATE()");
            });

            // 2. 產品表
            modelBuilder.Entity<ProductModel>(builder =>
            {
                // 若想自訂資料表名稱
                // builder.ToTable("Products");

                // 建立時間自動填入資料庫內部時間
                builder.Property(p => p.CreatedDate)
                       .HasDefaultValueSql("GETDATE()");
            });

            // 你還可以在此加入索引、複合鍵、資料庫關聯等設定
        }

        public DbSet<CouponModel> Coupons { get; set; }

    }
}