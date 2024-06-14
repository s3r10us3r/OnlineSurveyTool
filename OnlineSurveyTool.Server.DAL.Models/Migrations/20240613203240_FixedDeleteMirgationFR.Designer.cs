﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineSurveyTool.Server.DAL.Models;

#nullable disable

namespace OnlineSurveyTool.Server.DAL.Models.Migrations
{
    [DbContext(typeof(OSTDbContext))]
    [Migration("20240613203240_FixedDeleteMirgationFR")]
    partial class FixedDeleteMirgationFR
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("SingleChoiceOptionId")
                        .HasColumnType("int");

                    b.Property<int>("SurveyResultId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SingleChoiceOptionId");

                    b.HasIndex("SurveyResultId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.AnswerOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int>("ChoiceOptionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("ChoiceOptionId");

                    b.ToTable("AnswerOptions");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.ChoiceOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CanBeSkipped")
                        .HasColumnType("bit");

                    b.Property<double?>("Maximum")
                        .HasColumnType("float");

                    b.Property<double?>("Minimum")
                        .HasColumnType("float");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Survey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ClosingDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("OpeningDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.SurveyResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EMail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Login");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Answer", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.ChoiceOption", "SingleChoiceOption")
                        .WithMany("Answers")
                        .HasForeignKey("SingleChoiceOptionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.SurveyResult", "SurveyResult")
                        .WithMany("Answers")
                        .HasForeignKey("SurveyResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("SingleChoiceOption");

                    b.Navigation("SurveyResult");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.AnswerOption", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.Answer", "Answer")
                        .WithMany("AnswerOptions")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.ChoiceOption", "ChoiceOption")
                        .WithMany("AnswerOptions")
                        .HasForeignKey("ChoiceOptionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("ChoiceOption");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.ChoiceOption", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.Question", "Question")
                        .WithMany("ChoiceOptions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Question", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.Survey", "Survey")
                        .WithMany("Questions")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Survey", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.User", "Owner")
                        .WithMany("Surveys")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.SurveyResult", b =>
                {
                    b.HasOne("OnlineSurveyTool.Server.DAL.Models.Survey", "Survey")
                        .WithMany("Results")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Answer", b =>
                {
                    b.Navigation("AnswerOptions");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.ChoiceOption", b =>
                {
                    b.Navigation("AnswerOptions");

                    b.Navigation("Answers");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("ChoiceOptions");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.Survey", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.SurveyResult", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("OnlineSurveyTool.Server.DAL.Models.User", b =>
                {
                    b.Navigation("Surveys");
                });
#pragma warning restore 612, 618
        }
    }
}
