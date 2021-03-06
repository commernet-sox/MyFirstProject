﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PollyTest1;

namespace PollyTest1.Migrations
{
    [DbContext(typeof(PollyTestDbContext))]
    partial class PollyTestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CPC.DBCore.AuditEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EntityTypeName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AuditEntry");
                });

            modelBuilder.Entity("CPC.DBCore.AuditEntryProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuditEntryId")
                        .HasColumnType("int");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("RelationName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AuditEntryId");

                    b.ToTable("AuditEntryProperty");
                });

            modelBuilder.Entity("PollyTest1.Model.SZCompanyInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FZJG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FZRQ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ORG_CODE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QYMC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QYYWLX")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TYSHXYDM")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("XH")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YXQ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZZDJ")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZZLB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZZXL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZZZSH")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SZCompanyInfo");
                });

            modelBuilder.Entity("PollyTest1.Model.TestApi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("TestApi");
                });

            modelBuilder.Entity("CPC.DBCore.AuditEntryProperty", b =>
                {
                    b.HasOne("CPC.DBCore.AuditEntry", "Parent")
                        .WithMany("Properties")
                        .HasForeignKey("AuditEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
