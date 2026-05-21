using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee_Management_System.Infrastructures.Context.EntityTypeConfiguration;

public class AdminEntityTypeConfiguration : IEntityTypeConfiguration<AdminEntity>
{
    public void Configure(EntityTypeBuilder<AdminEntity> builder)
    {
        // テーブル名
       builder.ToTable("administrator");

       // 主キー
       builder.HasKey(a => a.UserId);

       // カラム
       builder.Property(a => a.UserId)
              .HasColumnName("user_id")
              .IsRequired();
       
       builder.Property(a => a.Password)
              .HasColumnName("password")
              .IsRequired();

       builder.Property(a => a.UserName)
              .HasColumnName("user_name")
              .IsRequired();
    }
}