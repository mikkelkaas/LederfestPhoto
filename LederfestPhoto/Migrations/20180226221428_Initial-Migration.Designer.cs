﻿// <auto-generated />
using LederfestPhoto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LederfestPhoto.Migrations
{
    [DbContext(typeof(LederfestPhotoContext))]
    [Migration("20180226221428_Initial-Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LederfestPhoto.Models.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Challenge");

                    b.Property<int>("Rating");

                    b.Property<Guid>("Team");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}