﻿// <auto-generated />
using System;
using ERPServices.CashFlow.API.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ERPServices.CashFlow.API.Migrations
{
    [DbContext(typeof(MySQLContext))]
    [Migration("20230504181344_SeedCashFlowDataTableOnDB")]
    partial class SeedCashFlowDataTableOnDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ERPServices.CashFlow.API.Model.CashFlowModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_inc");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("description");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("type");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("cash_flow");

                    b.HasData(
                        new
                        {
                            Id = 3L,
                            Date = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Lancamento Folha de Pagamento",
                            Type = "C",
                            Value = 10m
                        },
                        new
                        {
                            Id = 4L,
                            Date = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Lancamento Recebimento cliente",
                            Type = "D",
                            Value = 100m
                        },
                        new
                        {
                            Id = 2L,
                            Date = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Lancamento Recebimento cliente 2",
                            Type = "D",
                            Value = 100m
                        },
                        new
                        {
                            Id = 1L,
                            Date = new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Lancamento Recebimento cliente 3",
                            Type = "D",
                            Value = 100m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
