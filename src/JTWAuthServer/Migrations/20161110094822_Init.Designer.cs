using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using JTWAuthServer.Data;

namespace JTWAuthServer.Migrations
{
    [DbContext(typeof(JWTAuthDbContext))]
    [Migration("20161110094822_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("JTWAuthServer.Services.JWTClient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppId")
                        .IsRequired();

                    b.Property<string>("AppSecret")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOnDate");

                    b.Property<bool>("Enabled");

                    b.Property<string>("LastAccessToken");

                    b.Property<DateTime?>("LastModifiedOnDate");

                    b.Property<string>("LastRefreshToken");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("JWTApplication");
                });
        }
    }
}
