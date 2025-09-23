using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Models;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmpAttendanceDatum> EmpAttendanceData { get; set; }

    public virtual DbSet<EmpDatum> EmpData { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAttRecord> EmployeeAttRecords { get; set; }

    public virtual DbSet<TaigaAttendanceRecrd> TaigaAttendanceRecrds { get; set; }

    public virtual DbSet<TaigaTesting> TaigaTestings { get; set; }

    public virtual DbSet<Tview> Tviews { get; set; }

    public virtual DbSet<Workinghr> Workinghrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Harry;Database=TaigaApparel;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmpAttendanceDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Emp_Attendance_Data");

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmpDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Emp_Data");

            entity.Property(e => e.AccessDate).HasColumnType("datetime");
            entity.Property(e => e.AccessTime).HasColumnType("datetime");
            entity.Property(e => e.AuthenticationResult)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AuthenticationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeviceSrNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReaderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResourceName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Employee");

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeAttRecord>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Employee_AttRecord");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TaigaAttendanceRecrd>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Taiga-Attendance-Recrd");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TaigaTesting>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Taiga-Testing");

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tview");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Workinghr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("workinghrs");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MachineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserDate).HasColumnType("datetime");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
