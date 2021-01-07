﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.Infrastructure.Persistence;

namespace api.Migrations
{
    [DbContext(typeof(TodoListContext))]
    partial class TodoListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("api.Modules.TodoListModule.Domain.Models.TodoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TodoList");
                });

            modelBuilder.Entity("api.Modules.TodoListModule.Domain.Models.TodoListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Done")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("TodoListId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("TodoListItem");
                });

            modelBuilder.Entity("api.Modules.TodoListModule.Domain.Models.TodoListItem", b =>
                {
                    b.HasOne("api.Modules.TodoListModule.Domain.Models.TodoList", "TodoList")
                        .WithMany("Items")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("api.Modules.TodoListModule.Domain.Models.TodoList", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
