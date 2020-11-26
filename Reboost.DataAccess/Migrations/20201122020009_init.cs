using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reboost.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Annotations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(nullable: false),
                    ReviewId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    PageNumber = table.Column<int>(nullable: false),
                    TopPosition = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PageCount = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookUps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Raters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    FirstLanguage = table.Column<string>(nullable: true),
                    AppliedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Biography = table.Column<string>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestQueue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RequestedDatetime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewRankings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    StartScore = table.Column<int>(nullable: false),
                    EndScore = table.Column<int>(nullable: false),
                    AverageCompletionTime = table.Column<int>(nullable: false),
                    PriorityLevel = table.Column<int>(nullable: false),
                    MinimumRate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewRankings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ReviewId = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    ReviewerId = table.Column<string>(nullable: true),
                    RevieweeId = table.Column<string>(nullable: true),
                    FinalScore = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TimeSpentInSeconds = table.Column<int>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rubrics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sysdiagrams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    principal_id = table.Column<int>(nullable: false),
                    diagram_id = table.Column<int>(nullable: false),
                    version = table.Column<int>(nullable: true),
                    definition = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sysdiagrams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Focus = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Direction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InTextComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnotationId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    TopPosition = table.Column<int>(nullable: false),
                    AnnotationsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InTextComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InTextComments_Annotations_AnnotationsId",
                        column: x => x.AnnotationsId,
                        principalTable: "Annotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResponseTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    DisLikeCount = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseTemplates_QuestionTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    RankId = table.Column<int>(nullable: false),
                    RankingScore = table.Column<int>(nullable: false),
                    AverageReviewRate = table.Column<decimal>(nullable: false),
                    ReviewRankingsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRanks_ReviewRankings_ReviewRankingsId",
                        column: x => x.ReviewRankingsId,
                        principalTable: "ReviewRankings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(nullable: false),
                    CriteriaId = table.Column<int>(nullable: false),
                    Score = table.Column<decimal>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    ReviewsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewData_Reviews_ReviewsId",
                        column: x => x.ReviewsId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RubricId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MaxScore = table.Column<decimal>(nullable: false),
                    Weight = table.Column<int>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: false),
                    RubricsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RubricCriteria_Rubrics_RubricsId",
                        column: x => x.RubricsId,
                        principalTable: "Rubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubmissionCount = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    DisLikeCount = table.Column<int>(nullable: false),
                    HasSample = table.Column<bool>(nullable: false),
                    AverageScore = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaterCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaterId = table.Column<int>(nullable: false),
                    TestId = table.Column<int>(nullable: false),
                    CredentialType = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    FileExtension = table.Column<string>(nullable: true),
                    RatersId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaterCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaterCredentials_Raters_RatersId",
                        column: x => x.RatersId,
                        principalTable: "Raters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RaterCredentials_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MaxScores = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSections_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CriteriaValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriteriaId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RubricCriteriaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriteriaValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriteriaValues_RubricCriteria_RubricCriteriaId",
                        column: x => x.RubricCriteriaId,
                        principalTable: "RubricCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Direction = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionParts_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    DocId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    SubmittedDate = table.Column<DateTime>(nullable: false),
                    TimeSpentInSeconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Documents_DocId",
                        column: x => x.DocId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserScores_TestSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "TestSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserScores_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    SubmissionId = table.Column<int>(nullable: false),
                    FeedbackType = table.Column<string>(nullable: true),
                    RequestedDateTime = table.Column<DateTime>(nullable: false),
                    CompletedDateTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubmissionsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewRequests_Submissions_SubmissionsId",
                        column: x => x.SubmissionsId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriteriaValues_RubricCriteriaId",
                table: "CriteriaValues",
                column: "RubricCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_InTextComments_AnnotationsId",
                table: "InTextComments",
                column: "AnnotationsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionParts_QuestionId",
                table: "QuestionParts",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TaskId",
                table: "Questions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RaterCredentials_RatersId",
                table: "RaterCredentials",
                column: "RatersId");

            migrationBuilder.CreateIndex(
                name: "IX_RaterCredentials_TestId",
                table: "RaterCredentials",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseTemplates_TypeId",
                table: "ResponseTemplates",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewData_ReviewsId",
                table: "ReviewData",
                column: "ReviewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRequests_SubmissionsId",
                table: "ReviewRequests",
                column: "SubmissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriteria_RubricsId",
                table: "RubricCriteria",
                column: "RubricsId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_DocId",
                table: "Submissions",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_QuestionId",
                table: "Submissions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSections_TestId",
                table: "TestSections",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRanks_ReviewRankingsId",
                table: "UserRanks",
                column: "ReviewRankingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserScores_SectionId",
                table: "UserScores",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserScores_UserId",
                table: "UserScores",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriteriaValues");

            migrationBuilder.DropTable(
                name: "InTextComments");

            migrationBuilder.DropTable(
                name: "LookUps");

            migrationBuilder.DropTable(
                name: "QuestionParts");

            migrationBuilder.DropTable(
                name: "RaterCredentials");

            migrationBuilder.DropTable(
                name: "RequestQueue");

            migrationBuilder.DropTable(
                name: "ResponseTemplates");

            migrationBuilder.DropTable(
                name: "ReviewData");

            migrationBuilder.DropTable(
                name: "ReviewRatings");

            migrationBuilder.DropTable(
                name: "ReviewRequests");

            migrationBuilder.DropTable(
                name: "sysdiagrams");

            migrationBuilder.DropTable(
                name: "UserRanks");

            migrationBuilder.DropTable(
                name: "UserScores");

            migrationBuilder.DropTable(
                name: "RubricCriteria");

            migrationBuilder.DropTable(
                name: "Annotations");

            migrationBuilder.DropTable(
                name: "Raters");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "ReviewRankings");

            migrationBuilder.DropTable(
                name: "TestSections");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Rubrics");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
