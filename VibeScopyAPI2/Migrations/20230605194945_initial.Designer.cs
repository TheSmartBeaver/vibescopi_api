﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VibeScopyAPI.Infrastructure;

#nullable disable

namespace VibeScopyAPI2.Migrations
{
    [DbContext(typeof(UnitOfWorkToto))]
    [Migration("20230605194945_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VibeScopyAPI.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnswersFilamentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<short>("Value")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("AnswersFilamentId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.AnswersFilament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FilamentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FilamentValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ProfileId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ProfilePropositionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.HasIndex("ProfilePropositionId");

                    b.ToTable("AnswersFilament");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AWSPathS3")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.ProfileProposition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ProfilePropositions");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Answer", b =>
                {
                    b.HasOne("VibeScopyAPI.Models.AnswersFilament", null)
                        .WithMany("Answers")
                        .HasForeignKey("AnswersFilamentId");

                    b.HasOne("VibeScopyAPI.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.AnswersFilament", b =>
                {
                    b.HasOne("VibeScopyAPI.Models.Profile", null)
                        .WithMany("AnsweredFilament")
                        .HasForeignKey("ProfileId");

                    b.HasOne("VibeScopyAPI.Models.ProfileProposition", null)
                        .WithMany("AnswersFilaments")
                        .HasForeignKey("ProfilePropositionId");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Photo", b =>
                {
                    b.HasOne("VibeScopyAPI.Models.Profile", null)
                        .WithMany("Photos")
                        .HasForeignKey("ProfileId");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Profile", b =>
                {
                    b.HasOne("VibeScopyAPI.Models.Profile", null)
                        .WithMany("LikedUsers")
                        .HasForeignKey("ProfileId");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.ProfileProposition", b =>
                {
                    b.HasOne("VibeScopyAPI.Models.Profile", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.AnswersFilament", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.Profile", b =>
                {
                    b.Navigation("AnsweredFilament");

                    b.Navigation("LikedUsers");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("VibeScopyAPI.Models.ProfileProposition", b =>
                {
                    b.Navigation("AnswersFilaments");
                });
#pragma warning restore 612, 618
        }
    }
}