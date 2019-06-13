using System;
using Master_v2.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Master_v2.Server.Models
{
    public partial class OrderMasterContext : DbContext
    {
        public OrderMasterContext()
        {
        }

        public OrderMasterContext(DbContextOptions<OrderMasterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<BillArticle> BillArticle { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreArticle> StoreArticle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-VF53KDE;Initial Catalog=OrderMaster;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bill_Person");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bill_Store");
            });

            modelBuilder.Entity<BillArticle>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.BillArticle)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillArticle_Article");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillArticle)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BillArticle_Bill");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Oib)
                    .IsRequired()
                    .HasColumnName("OIB")
                    .HasMaxLength(11);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Role");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_Store_Person");
            });

            modelBuilder.Entity<StoreArticle>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.StoreArticle)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreArticle_Article");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreArticle)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreArticle_Store");
            });
        }
    }
}
