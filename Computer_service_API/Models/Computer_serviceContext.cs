using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Computer_service_API.Models
{
    public partial class Computer_serviceContext : DbContext
    {
        public Computer_serviceContext()
        {
        }

        public Computer_serviceContext(DbContextOptions<Computer_serviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acquisition> Acquisitions { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Component> Components { get; set; } = null!;
        public virtual DbSet<ComponentType> ComponentTypes { get; set; } = null!;
        public virtual DbSet<ComponentUsage> ComponentUsages { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<IssueStatus> IssueStatuses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<OrderType> OrderTypes { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<ShortOrderInfo> ShortOrderInfos { get; set; } = null!;
        public virtual DbSet<ShortVacancyInfo> ShortVacancyInfos { get; set; } = null!;
        public virtual DbSet<Vacancy> Vacancies { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acquisition>(entity =>
            {
                entity.HasKey(e => e.AcqId)
                    .HasName("PK_ACQUISITION");

                entity.ToTable("Acquisition");

                entity.Property(e => e.AcqId).HasColumnName("acq_id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.Component)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("component");

                entity.Property(e => e.Price).HasColumnName("price");

            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PK_CLIENT");

                entity.ToTable("Client");

                entity.HasIndex(e => e.Login, "UQ__Client__7838F272A1B6425A")
                    .IsUnique();

                entity.Property(e => e.Login)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("login");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token")
                    .HasDefaultValueSql("('invalid_key')");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasKey(e => e.ArticleNum)
                    .HasName("PK_COMPONENT");

                entity.ToTable("Component");

                entity.HasIndex(e => e.ArticleNum, "UQ__Componen__BC6ED866BF16718F")
                    .IsUnique();

                entity.Property(e => e.ArticleNum)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("article_num");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Type).HasColumnName("type");

            });

            modelBuilder.Entity<ComponentType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_COMPONENT_TYPE");

                entity.ToTable("Component_type");

                entity.HasIndex(e => e.TypeName, "UQ__Componen__543C4FD9A5E9093E")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<ComponentUsage>(entity =>
            {
                entity.HasKey(e => e.UsageId)
                    .HasName("PK_COMPONENT_USAGE");

                entity.ToTable("Component_usage");

                entity.Property(e => e.UsageId).HasColumnName("usage_id");

                entity.Property(e => e.Component)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("component");

                entity.Property(e => e.Order).HasColumnName("order");

            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepId)
                    .HasName("PK_DEPARTMENT");

                entity.ToTable("Department");

                entity.HasIndex(e => e.DepName, "UQ__Departme__7BE5495069AF700F")
                    .IsUnique();

                entity.Property(e => e.DepId).HasColumnName("dep_id");

                entity.Property(e => e.DepName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("dep_name");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK_EMPLOYEE");

                entity.ToTable("Employee");

                entity.HasIndex(e => e.ServiceId, "UQ__Employee__3E0DB8AE23ABD04C")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "UQ__Employee__7838F272B752AA83")
                    .IsUnique();

                entity.Property(e => e.ServiceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("service_id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Department).HasColumnName("department");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.Login)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("second_name");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token")
                    .HasDefaultValueSql("('invalid_key')");

            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.IssId)
                    .HasName("PK_ISSUES");

                entity.Property(e => e.IssId).HasColumnName("iss_id");

                entity.Property(e => e.IssAssistant)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("iss_assistant");

                entity.Property(e => e.IssAuthor)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("iss_author");

                entity.Property(e => e.IssStatus).HasColumnName("iss_status");

            });

            modelBuilder.Entity<IssueStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_ISSUE_STATUS");

                entity.ToTable("Issue_status");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("status_name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("client");

                entity.Property(e => e.Employee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("employee");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");

            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.HasKey(e => e.HistId)
                    .HasName("PK_ORDER_HISTORY");

                entity.ToTable("Order_history");

                entity.Property(e => e.HistId).HasColumnName("hist_id");

                entity.Property(e => e.HistClient)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("hist_Client");

                entity.Property(e => e.HistDate)
                    .HasColumnType("date")
                    .HasColumnName("hist_date");

                entity.Property(e => e.HistOrder).HasColumnName("hist_order");

            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_ORDER_STATUS");

                entity.ToTable("Order_status");

                entity.HasIndex(e => e.StatusName, "UQ__Order_st__501B375313D68339")
                    .IsUnique();

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("status_id");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("status_name");
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.HasKey(e => e.Type)
                    .HasName("PK_ORDER_TYPE");

                entity.ToTable("order_type");

                entity.HasIndex(e => e.Type, "UQ__order_ty__E3F852485F662E8B")
                    .IsUnique();

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.BasePrice).HasColumnName("base_price");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.RevId)
                    .HasName("PK_REVIEWS");

                entity.Property(e => e.RevId).HasColumnName("rev_id");

                entity.Property(e => e.RevAuthor)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("rev_author");

                entity.Property(e => e.RevText)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("rev_text");

            });

            modelBuilder.Entity<ShortOrderInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Short_Order_Info");

                entity.Property(e => e.ClientLogin)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Client_Login");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(101)
                    .IsUnicode(false)
                    .HasColumnName("Employee_Name");

                entity.Property(e => e.OrderId).HasColumnName("Order_ID");

                entity.Property(e => e.OrderPrice).HasColumnName("Order_Price");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Order_Status");

                entity.Property(e => e.OrderType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Order_Type");
            });

            modelBuilder.Entity<ShortVacancyInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Short_Vacancy_Info");

                entity.Property(e => e.DepName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("dep_name");

                entity.Property(e => e.VacComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("vac_comment");

                entity.Property(e => e.VacId).HasColumnName("vac_id");

                entity.Property(e => e.VacSalary).HasColumnName("vac_salary");
            });

            modelBuilder.Entity<Vacancy>(entity =>
            {
                entity.HasKey(e => e.VacId)
                    .HasName("PK_VACANCY");

                entity.ToTable("vacancy");

                entity.Property(e => e.VacId).HasColumnName("vac_id");

                entity.Property(e => e.VacComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("vac_comment");

                entity.Property(e => e.VacDepartment).HasColumnName("vac_department");

                entity.Property(e => e.VacSalary).HasColumnName("vac_salary");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
