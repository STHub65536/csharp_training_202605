using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee_Management_System.Infrastructures.Context.EntityTypeConfiguration;
public class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        // テーブル名
        builder.ToTable("department");

        // 主キー
        builder.HasKey(d => d.DeptNo);

        // カラム
        builder.Property(d => d.DeptNo)
                .HasColumnName("dept_no")
                .IsRequired();

        builder.Property(d => d.DeptName)
                .HasColumnName("dept_name")
                .IsRequired();
    }
}