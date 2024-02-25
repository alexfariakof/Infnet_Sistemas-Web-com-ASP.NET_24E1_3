﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Migrations_MsSqlServer.Migrations
{
    [DbContext(typeof(RegisterContext))]
    [Migration("20240225162209_Fix_Address_Model")]
    partial class Fix_Address_Model
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Account.Agreggates.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birth")
                        .HasColumnType("datetime2");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Merchant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Merchant", (string)null);
                });

            modelBuilder.Entity("Domain.Account.Agreggates.PlaylistPersonal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AlbumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DtCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("CustomerId");

                    b.ToTable("PlaylistPersonal", (string)null);
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Signature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DtActivation")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FlatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("FlatId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Signature", (string)null);
                });

            modelBuilder.Entity("Domain.Account.ValueObject.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Complement")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Neighborhood")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DestinationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DtNotification")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("DestinationId");

                    b.HasIndex("MerchantId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("BandId");

                    b.ToTable("Album", (string)null);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Band", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Backdrop")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Band", (string)null);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Flat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Flat", (string)null);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Music", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AlbumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.ToTable("Music", (string)null);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Playlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FlatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.ToTable("Playlist", (string)null);
                });

            modelBuilder.Entity("Domain.Transactions.Agreggates.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("CardBrandId")
                        .HasColumnType("int");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.HasKey("Id");

                    b.HasIndex("CardBrandId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Card", (string)null);
                });

            modelBuilder.Entity("Domain.Transactions.Agreggates.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DtTransaction")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("Domain.Transactions.ValueObject.CreditCardBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CardBrand", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 99,
                            Name = "Invalid"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Visa"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mastercard"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Amex"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Discover"
                        },
                        new
                        {
                            Id = 5,
                            Name = "DinersClub"
                        },
                        new
                        {
                            Id = 6,
                            Name = "JCB"
                        });
                });

            modelBuilder.Entity("MusicPlayList", b =>
                {
                    b.Property<Guid>("MusicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DtAdded")
                        .HasColumnType("datetime2");

                    b.HasKey("MusicId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("MusicPlayList");
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Customer", b =>
                {
                    b.OwnsOne("Domain.Account.ValueObject.Login", "Login", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Email");

                            b1.Property<string>("Password")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("Password");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("Domain.Account.ValueObject.Phone", "Phone", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Phone");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Login");

                    b.Navigation("Phone");
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Merchant", b =>
                {
                    b.OwnsOne("Domain.Account.ValueObject.Login", "Login", b1 =>
                        {
                            b1.Property<Guid>("MerchantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Email");

                            b1.Property<string>("Password")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("nvarchar(255)")
                                .HasColumnName("Password");

                            b1.HasKey("MerchantId");

                            b1.ToTable("Merchant");

                            b1.WithOwner()
                                .HasForeignKey("MerchantId");
                        });

                    b.OwnsOne("Domain.Account.ValueObject.Phone", "Phone", b1 =>
                        {
                            b1.Property<Guid>("MerchantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("Phone");

                            b1.HasKey("MerchantId");

                            b1.ToTable("Merchant");

                            b1.WithOwner()
                                .HasForeignKey("MerchantId");
                        });

                    b.Navigation("Login");

                    b.Navigation("Phone");
                });

            modelBuilder.Entity("Domain.Account.Agreggates.PlaylistPersonal", b =>
                {
                    b.HasOne("Domain.Streaming.Agreggates.Album", null)
                        .WithMany("MusicPersonal")
                        .HasForeignKey("AlbumId");

                    b.HasOne("Domain.Account.Agreggates.Customer", "Customer")
                        .WithMany("Playlists")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Signature", b =>
                {
                    b.HasOne("Domain.Account.Agreggates.Customer", null)
                        .WithMany("Signatures")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Domain.Streaming.Agreggates.Flat", "Flat")
                        .WithMany()
                        .HasForeignKey("FlatId");

                    b.HasOne("Domain.Account.Agreggates.Merchant", null)
                        .WithMany("Signatures")
                        .HasForeignKey("MerchantId");

                    b.Navigation("Flat");
                });

            modelBuilder.Entity("Domain.Account.ValueObject.Address", b =>
                {
                    b.HasOne("Domain.Account.Agreggates.Customer", null)
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Domain.Account.Agreggates.Merchant", null)
                        .WithMany("Addresses")
                        .HasForeignKey("MerchantId");
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.HasOne("Domain.Account.Agreggates.Customer", "Destination")
                        .WithMany("Notifications")
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Account.Agreggates.Merchant", null)
                        .WithMany("Notifications")
                        .HasForeignKey("MerchantId");

                    b.HasOne("Domain.Account.Agreggates.Customer", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("Destination");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Album", b =>
                {
                    b.HasOne("Domain.Streaming.Agreggates.Band", null)
                        .WithMany("Albums")
                        .HasForeignKey("BandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Flat", b =>
                {
                    b.OwnsOne("Domain.Core.ValueObject.Monetary", "Value", b1 =>
                        {
                            b1.Property<Guid>("FlatId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Monetary");

                            b1.HasKey("FlatId");

                            b1.ToTable("Flat");

                            b1.WithOwner()
                                .HasForeignKey("FlatId");
                        });

                    b.Navigation("Value")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Music", b =>
                {
                    b.HasOne("Domain.Streaming.Agreggates.Album", null)
                        .WithMany("Music")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Domain.Streaming.ValueObject.Duration", "Duration", b1 =>
                        {
                            b1.Property<Guid>("MusicId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Value")
                                .HasMaxLength(50)
                                .HasColumnType("int")
                                .HasColumnName("Duration");

                            b1.HasKey("MusicId");

                            b1.ToTable("Music");

                            b1.WithOwner()
                                .HasForeignKey("MusicId");
                        });

                    b.Navigation("Duration")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Playlist", b =>
                {
                    b.HasOne("Domain.Streaming.Agreggates.Flat", "Flat")
                        .WithMany()
                        .HasForeignKey("FlatId");

                    b.Navigation("Flat");
                });

            modelBuilder.Entity("Domain.Transactions.Agreggates.Card", b =>
                {
                    b.HasOne("Domain.Transactions.ValueObject.CreditCardBrand", "CardBrand")
                        .WithMany("Cards")
                        .HasForeignKey("CardBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Account.Agreggates.Customer", null)
                        .WithMany("Cards")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Domain.Account.Agreggates.Merchant", null)
                        .WithMany("Cards")
                        .HasForeignKey("MerchantId");

                    b.OwnsOne("Domain.Core.ValueObject.Monetary", "Limit", b1 =>
                        {
                            b1.Property<Guid>("CardId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Limit");

                            b1.HasKey("CardId");

                            b1.ToTable("Card");

                            b1.WithOwner()
                                .HasForeignKey("CardId");
                        });

                    b.OwnsOne("Domain.Transactions.ValueObject.ExpiryDate", "Validate", b1 =>
                        {
                            b1.Property<Guid>("CardId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("datetime2")
                                .HasColumnName("Validate");

                            b1.HasKey("CardId");

                            b1.ToTable("Card");

                            b1.WithOwner()
                                .HasForeignKey("CardId");
                        });

                    b.Navigation("CardBrand");

                    b.Navigation("Limit")
                        .IsRequired();

                    b.Navigation("Validate");
                });

            modelBuilder.Entity("Domain.Transactions.Agreggates.Transaction", b =>
                {
                    b.HasOne("Domain.Transactions.Agreggates.Card", null)
                        .WithMany("Transactions")
                        .HasForeignKey("CardId");

                    b.HasOne("Domain.Account.Agreggates.Customer", "Customer")
                        .WithMany("Transactions")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Account.Agreggates.Merchant", null)
                        .WithMany("Transactions")
                        .HasForeignKey("MerchantId");

                    b.OwnsOne("Domain.Core.ValueObject.Monetary", "Value", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Value")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Monetary");

                            b1.HasKey("TransactionId");

                            b1.ToTable("Transaction");

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Value")
                        .IsRequired();
                });

            modelBuilder.Entity("MusicPlayList", b =>
                {
                    b.HasOne("Domain.Streaming.Agreggates.Music", null)
                        .WithMany()
                        .HasForeignKey("MusicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Account.Agreggates.PlaylistPersonal", null)
                        .WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Streaming.Agreggates.Playlist", null)
                        .WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Customer", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Cards");

                    b.Navigation("Notifications");

                    b.Navigation("Playlists");

                    b.Navigation("Signatures");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Domain.Account.Agreggates.Merchant", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Cards");

                    b.Navigation("Notifications");

                    b.Navigation("Signatures");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Album", b =>
                {
                    b.Navigation("Music");

                    b.Navigation("MusicPersonal");
                });

            modelBuilder.Entity("Domain.Streaming.Agreggates.Band", b =>
                {
                    b.Navigation("Albums");
                });

            modelBuilder.Entity("Domain.Transactions.Agreggates.Card", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Domain.Transactions.ValueObject.CreditCardBrand", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
