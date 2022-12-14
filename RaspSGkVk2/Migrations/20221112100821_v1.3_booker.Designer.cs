// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RaspSGkVk2.Prop;

#nullable disable

namespace RaspSGkVk2.Migrations
{
    [DbContext(typeof(BotDB))]
    [Migration("20221112100821_v1.3_booker")]
    partial class v13_booker
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("RaspSGkVk2.Prop.Settings", b =>
                {
                    b.Property<int>("IdSetting")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("IdGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Timer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TokenVk")
                        .HasColumnType("TEXT");

                    b.HasKey("IdSetting");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("RaspSGkVk2.Prop.Tasks", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("PeerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultText")
                        .HasColumnType("TEXT");

                    b.Property<char?>("TypeTask")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("IdTask");

                    b.ToTable("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
