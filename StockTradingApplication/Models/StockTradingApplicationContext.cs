using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StockTradingApplication.Models;

public partial class StockTradingApplicationContext : DbContext
{
    public StockTradingApplicationContext()
    {
    }

    public StockTradingApplicationContext(DbContextOptions<StockTradingApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<TblAccountDetail> TblAccountDetails { get; set; }

    public virtual DbSet<TblBank> TblBanks { get; set; }

    public virtual DbSet<TblBlog> TblBlogs { get; set; }

    public virtual DbSet<TblModule> TblModules { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblStock> TblStocks { get; set; }

    public virtual DbSet<TblStockTransactionDetail> TblStockTransactionDetails { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserBankDetail> TblUserBankDetails { get; set; }

    public virtual DbSet<TblUserStockDetail> TblUserStockDetails { get; set; }

    public virtual DbSet<TblUserStockSearchHistory> TblUserStockSearchHistories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Address).HasMaxLength(512);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        });

        modelBuilder.Entity<TblAccountDetail>(entity =>
        {
            entity.ToTable("tblAccountDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountNo).HasDefaultValue("");
        });

        modelBuilder.Entity<TblBank>(entity =>
        {
            entity.ToTable("tblBanks");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.ToTable("tblBlogs");
        });

        modelBuilder.Entity<TblModule>(entity =>
        {
            entity.ToTable("tblModules");

            entity.Property(e => e.IsDelete)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.ToTable("tblPermissions");
        });

        modelBuilder.Entity<TblStock>(entity =>
        {
            entity.ToTable("tblStocks");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Exchange).HasDefaultValue("");
            entity.Property(e => e.IsHaramRevenueOver5Per)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
            entity.Property(e => e.Mcap).HasColumnName("MCap");
            entity.Property(e => e.NotComplianceSectors).HasDefaultValue("");
        });

        modelBuilder.Entity<TblStockTransactionDetail>(entity =>
        {
            entity.ToTable("tblStockTransactionDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrderType).HasDefaultValue("");
            entity.Property(e => e.TimeInForce).HasDefaultValue("");
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.ToTable("tblTransaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UserBankAccountNo).HasDefaultValue("");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("tblUsers");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsDelete)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
        });

        modelBuilder.Entity<TblUserBankDetail>(entity =>
        {
            entity.ToTable("tblUserBankDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountHolderName).HasDefaultValue("");
        });

        modelBuilder.Entity<TblUserStockDetail>(entity =>
        {
            entity.ToTable("tblUserStockDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblUserStockSearchHistory>(entity =>
        {
            entity.ToTable("tblUserStockSearchHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
