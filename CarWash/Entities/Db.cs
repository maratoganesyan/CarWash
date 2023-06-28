using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarWash.Entities
{
    public partial class Db : DbContext
    {
        public virtual DbSet<AdditionalServices> AdditionalServices { get; set; }
        public virtual DbSet<Body> Body { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ClientsCars> ClientsCars { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Models> Models { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<ServicesInOrder> ServicesInOrder { get; set; }

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public Db()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer("Data Source=HOKAGE\\SQLEXPRESS;" +
                    "Initial Catalog=CarWash;Integrated Security=True;" +
                    "Multiple Active Result Sets=True;Trust Server Certificate=True;" +
                    "Command Timeout=300");

                //"Initial Catalog=...;Integrated Security=True;" +
                //"Trust Server Certificate=True;Command Timeout=300;" +
                //"MultipleActiveResultSets=true;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalServices>(entity =>
            {
                entity.HasKey(e => e.IdService);

                entity.Property(e => e.ServiceDescription).IsRequired();

                entity.Property(e => e.ServiceName).IsRequired();

                entity.Property(e => e.ServicePrice).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Body>(entity =>
            {
                entity.HasKey(e => e.IdBody);

                entity.Property(e => e.BodyName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.IdCar);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.StateNumber)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.HasOne(d => d.IdBodyNavigation)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.IdBody)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Car_Body");

                entity.HasOne(d => d.IdColorNavigation)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.IdColor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Car_Color");

                entity.HasOne(d => d.IdModelNavigation)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.IdModel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Car_Models1");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasKey(e => e.IdClient);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdGenderNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.IdGender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clients_Gender");
            });

            modelBuilder.Entity<ClientsCars>(entity =>
            {
                entity.HasKey(e => new { e.IdCar, e.IdClient });

                entity.HasOne(d => d.IdCarNavigation)
                    .WithMany(p => p.ClientsCars)
                    .HasForeignKey(d => d.IdCar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientsCars_Car");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.ClientsCars)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientsCars_Clients");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.IdColor);

                entity.Property(e => e.ColorDescription).IsRequired();

                entity.Property(e => e.ColorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Hex)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("HEX");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdGenderNavigation)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.IdGender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Gender");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Role");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.IdGender);

                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(e => e.IdMark);

                entity.Property(e => e.MarkName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Models>(entity =>
            {
                entity.HasKey(e => e.IdModel);

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdMarkNavigation)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.IdMark)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_Mark");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder);

                entity.Property(e => e.DateTimeOfOrder).HasColumnType("datetime");

                entity.Property(e => e.TotalPriceOfOrder).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Employee");

                entity.HasOne(d => d.IdOrderStatusNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.IdOrderStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OrderStatus");

                entity.HasOne(d => d.IdC)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => new { d.IdCar, d.IdClient })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_ClientsCars");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.IdOrderStatus);

                entity.Property(e => e.OrderStatusName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ServicesInOrder>(entity =>
            {
                entity.HasKey(e => new { e.IdOrder, e.IdService });

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.ServicesInOrder)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicesInOrder_Order");

                entity.HasOne(d => d.IdServiceNavigation)
                    .WithMany(p => p.ServicesInOrder)
                    .HasForeignKey(d => d.IdService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServicesInOrder_AdditionalServices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
