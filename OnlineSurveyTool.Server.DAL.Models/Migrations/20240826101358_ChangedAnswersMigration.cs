using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSurveyTool.Server.DAL.Models.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAnswersMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpeningDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ClosingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsOpen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsArchived = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SurveyId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Minimum = table.Column<double>(type: "double", nullable: true),
                    Maximum = table.Column<double>(type: "double", nullable: true),
                    CanBeSkipped = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SurveyId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuestionId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    SurveyResultId = table.Column<int>(type: "int", nullable: false),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "varchar(21)", maxLength: 21, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuestionId = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResultId = table.Column<int>(type: "int", nullable: true),
                    Answer = table.Column<double>(type: "double", nullable: true),
                    AnswerNumerical_ResultId = table.Column<int>(type: "int", nullable: true),
                    ChoiceOptionId = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnswerSingleChoice_ResultId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "varchar(10000)", maxLength: 10000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnswerTextual_ResultId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => new { x.SurveyResultId, x.QuestionNumber });
                    table.ForeignKey(
                        name: "FK_Answer_Answers_ChoiceOptionId",
                        column: x => x.ChoiceOptionId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Results_AnswerNumerical_ResultId",
                        column: x => x.AnswerNumerical_ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Results_AnswerSingleChoice_ResultId",
                        column: x => x.AnswerSingleChoice_ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Results_AnswerTextual_ResultId",
                        column: x => x.AnswerTextual_ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResultId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    ResultId0 = table.Column<int>(name: "$ResultId", type: "int", nullable: false),
                    ChoiceOptionId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Answer_$ResultId_Number",
                        columns: x => new { x.ResultId0, x.Number },
                        principalTable: "Answer",
                        principalColumns: new[] { "SurveyResultId", "QuestionNumber" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Answers_ChoiceOptionId",
                        column: x => x.ChoiceOptionId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerNumerical_ResultId",
                table: "Answer",
                column: "AnswerNumerical_ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerSingleChoice_ResultId",
                table: "Answer",
                column: "AnswerSingleChoice_ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswerTextual_ResultId",
                table: "Answer",
                column: "AnswerTextual_ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ChoiceOptionId",
                table: "Answer",
                column: "ChoiceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ResultId",
                table: "Answer",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_$ResultId_Number",
                table: "AnswerOptions",
                columns: new[] { "$ResultId", "Number" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_ChoiceOptionId",
                table: "AnswerOptions",
                column: "ChoiceOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId",
                table: "Questions",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_SurveyId",
                table: "Results",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_OwnerId",
                table: "Surveys",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
