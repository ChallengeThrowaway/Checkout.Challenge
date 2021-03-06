﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentGateway.Data;

namespace PaymentGateway.Data.Migrations
{
    [DbContext(typeof(PaymentGatewayContext))]
    partial class PaymentGatewayContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PaymentGateway.Data.Entities.ApiKey", b =>
                {
                    b.Property<Guid>("ApiKeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OwnerMerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("ValidFrom")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ValidUntil")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ApiKeyId");

                    b.HasIndex("OwnerMerchantId");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.CardDetails", b =>
                {
                    b.Property<Guid>("CardDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardholderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cvv")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardDetailsId");

                    b.ToTable("CardDetails");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Merchant", b =>
                {
                    b.Property<Guid>("MerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MerchantName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantId");

                    b.ToTable("Merchants");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("BankId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CardDetailsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrencyIsoAlpha3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PaymentId");

                    b.HasIndex("CardDetailsId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.PaymentStatus", b =>
                {
                    b.Property<Guid>("PaymentStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("StatusDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("StatusKey")
                        .HasColumnType("int");

                    b.HasKey("PaymentStatusId");

                    b.HasIndex("PaymentId");

                    b.ToTable("PaymentStatuses");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.ApiKey", b =>
                {
                    b.HasOne("PaymentGateway.Data.Entities.Merchant", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerMerchantId");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.Payment", b =>
                {
                    b.HasOne("PaymentGateway.Data.Entities.CardDetails", "CardDetails")
                        .WithMany()
                        .HasForeignKey("CardDetailsId");

                    b.HasOne("PaymentGateway.Data.Entities.Merchant", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId");
                });

            modelBuilder.Entity("PaymentGateway.Data.Entities.PaymentStatus", b =>
                {
                    b.HasOne("PaymentGateway.Data.Entities.Payment", null)
                        .WithMany("PaymentStatuses")
                        .HasForeignKey("PaymentId");
                });
#pragma warning restore 612, 618
        }
    }
}
