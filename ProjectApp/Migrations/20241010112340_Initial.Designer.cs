﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectApp.Persistence;

#nullable disable

namespace ProjectApp.Migrations
{
    [DbContext(typeof(AuctionDbContext))]
    [Migration("20241010112340_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectApp.Persistance.AuctionDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AuctionEndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("StartPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("AuctionsDbs");
                });

            modelBuilder.Entity("ProjectApp.Persistance.BidDb", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfBid")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("BidId");

                    b.HasIndex("AuctionId");

                    b.ToTable("BidDbs");
                });

            modelBuilder.Entity("ProjectApp.Persistance.BidDb", b =>
                {
                    b.HasOne("ProjectApp.Persistance.AuctionDb", "Auction")
                        .WithMany("BidDbs")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("ProjectApp.Persistance.AuctionDb", b =>
                {
                    b.Navigation("BidDbs");
                });
#pragma warning restore 612, 618
        }
    }
}
