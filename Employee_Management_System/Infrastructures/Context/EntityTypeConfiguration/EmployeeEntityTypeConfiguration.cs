using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee_Management_System.Infrastructures.Context.EntityTypeConfiguration;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        // テーブル名
       builder.ToTable("employee");

       // 主キー
       builder.HasKey(e => e.EmpNo);

       // カラム
       builder.Property(e => e.EmpNo)
              .HasColumnName("emp_no");

       builder.Property(e => e.EmpName)
              .HasColumnName("emp_name")
              .IsRequired();

       builder.Property(e => e.Birthday)
              .HasColumnName("birthday")
              .IsRequired();

       builder.Property(e => e.MailAddress)
              .HasColumnName("mail_address")
              .IsRequired();

       builder.Property(e => e.DeptNo)
              .HasColumnName("dept_no");

       builder.HasOne(e => e.Dept)
              .WithMany(d => d.Employees)
              .HasForeignKey(e => e.DeptNo)
              .OnDelete(DeleteBehavior.Restrict);
    }
}