﻿// <auto-generated />
using System;
using GoPlayServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GoPlayServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("AppUserGroup", b =>
                {
                    b.Property<Guid>("groupsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("usersId")
                        .HasColumnType("TEXT");

                    b.HasKey("groupsId", "usersId");

                    b.HasIndex("usersId");

                    b.ToTable("AppUserGroup");
                });

            modelBuilder.Entity("GoPlayServer.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("age")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("banned")
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("firstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("lastName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("mutedFor")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("mutedOn")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("sports")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("GoPlayServer.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("groupName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("GoPlayServer.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("GoPlayServer.Entities.NewsPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("heading")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("pictureUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timeOfCreation")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NewsPosts");
                });

            modelBuilder.Entity("GoPlayServer.Entities.PlayPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("groupName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("heading")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timeOfCreation")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PlayPosts");
                });

            modelBuilder.Entity("GoPlayServer.Entities.ReportedPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("reportedPostId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("reporterId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReportedPosts");
                });

            modelBuilder.Entity("AppUserGroup", b =>
                {
                    b.HasOne("GoPlayServer.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("groupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoPlayServer.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("usersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GoPlayServer.Entities.Message", b =>
                {
                    b.HasOne("GoPlayServer.Entities.Group", "group")
                        .WithMany("messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("group");
                });

            modelBuilder.Entity("GoPlayServer.Entities.Group", b =>
                {
                    b.Navigation("messages");
                });
#pragma warning restore 612, 618
        }
    }
}
