using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using JTWAuthServer.Data;

namespace JTWAuthServer.Migrations
{
    [DbContext(typeof(JWTAuthDbContext))]
    partial class JWTAuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("JTWAuthServer.Services.JWTApplication", b =>
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

                    b.HasKey("Id");

                    b.ToTable("JWTApplication");
                });
        }
    }
}
