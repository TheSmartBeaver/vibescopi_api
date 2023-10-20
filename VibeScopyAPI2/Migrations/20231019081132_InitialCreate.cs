using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace VibeScopyAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ActivityCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    AuthentUid = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    SubscriptionType = table.Column<int>(type: "integer", nullable: false),
                    Langages = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.AuthentUid);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionFilament",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionFilament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LovingGenders = table.Column<string>(type: "text", nullable: false),
                    FriendGenders = table.Column<string>(type: "text", nullable: false),
                    LookingRelationShips = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    AWSPathS3 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "AuthentUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePropositions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LastLocation = table.Column<Point>(type: "geography (point)", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Hobbies = table.Column<string>(type: "text", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePropositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePropositions_Profiles_Id",
                        column: x => x.Id,
                        principalTable: "Profiles",
                        principalColumn: "AuthentUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SwipedUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileAuthentUid = table.Column<string>(type: "text", nullable: true),
                    SwipeStatus = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwipedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SwipedUser_Profiles_ProfileAuthentUid",
                        column: x => x.ProfileAuthentUid,
                        principalTable: "Profiles",
                        principalColumn: "AuthentUid");
                });

            migrationBuilder.CreateTable(
                name: "UserLikeProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<string>(type: "text", nullable: false),
                    LikedPersonId = table.Column<string>(type: "text", nullable: false),
                    RateAction = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeProfiles", x => new { x.UserProfileId, x.LikedPersonId });
                    table.ForeignKey(
                        name: "FK_UserLikeProfiles_Profiles_LikedPersonId",
                        column: x => x.LikedPersonId,
                        principalTable: "Profiles",
                        principalColumn: "AuthentUid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeProfiles_Profiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "Profiles",
                        principalColumn: "AuthentUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PossibleAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    QuestionFilamentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PossibleAnswer_QuestionFilament_QuestionFilamentId",
                        column: x => x.QuestionFilamentId,
                        principalTable: "QuestionFilament",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswersFilaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<string>(type: "text", nullable: false),
                    QuestionFilamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilamentValue = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProfilePropositionId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswersFilaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswersFilaments_ProfilePropositions_ProfilePropositionId",
                        column: x => x.ProfilePropositionId,
                        principalTable: "ProfilePropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnswersFilaments_QuestionFilament_QuestionFilamentId",
                        column: x => x.QuestionFilamentId,
                        principalTable: "QuestionFilament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    AnswersFilamentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AnswersFilaments_AnswersFilamentId",
                        column: x => x.AnswersFilamentId,
                        principalTable: "AnswersFilaments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_AnswersFilamentId",
                table: "Answer",
                column: "AnswersFilamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswersFilaments_ProfilePropositionId",
                table: "AnswersFilaments",
                column: "ProfilePropositionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswersFilaments_QuestionFilamentId",
                table: "AnswersFilaments",
                column: "QuestionFilamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProfileId",
                table: "Photos",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PossibleAnswer_QuestionFilamentId",
                table: "PossibleAnswer",
                column: "QuestionFilamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AuthentUid",
                table: "Profiles",
                column: "AuthentUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Email",
                table: "Profiles",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SwipedUser_ProfileAuthentUid",
                table: "SwipedUser",
                column: "ProfileAuthentUid");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeProfiles_LikedPersonId",
                table: "UserLikeProfiles",
                column: "LikedPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PossibleAnswer");

            migrationBuilder.DropTable(
                name: "SwipedUser");

            migrationBuilder.DropTable(
                name: "UserLikeProfiles");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "AnswersFilaments");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "ProfilePropositions");

            migrationBuilder.DropTable(
                name: "QuestionFilament");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
