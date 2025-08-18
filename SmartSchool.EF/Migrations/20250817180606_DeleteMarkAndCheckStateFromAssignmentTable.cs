using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.EF.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMarkAndCheckStateFromAssignmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_SubjectDetails_SubjectDetailsId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_SubjectDetailsId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ChekeState",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "SubjectDetailsId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "SubmitedDate",
                table: "Assignments",
                newName: "UploadDate");

            migrationBuilder.AddColumn<string>(
                name: "ChekeState",
                table: "SubmittedAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubjectDetailId1",
                table: "Assignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectDetailId1",
                table: "Assignments",
                column: "SubjectDetailId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_SubjectDetails_SubjectDetailId1",
                table: "Assignments",
                column: "SubjectDetailId1",
                principalTable: "SubjectDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_SubjectDetails_SubjectDetailId1",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_SubjectDetailId1",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ChekeState",
                table: "SubmittedAssignments");

            migrationBuilder.DropColumn(
                name: "SubjectDetailId1",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "Assignments",
                newName: "SubmitedDate");

            migrationBuilder.AddColumn<string>(
                name: "ChekeState",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Mark",
                table: "Assignments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectDetailsId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectDetailsId",
                table: "Assignments",
                column: "SubjectDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_SubjectDetails_SubjectDetailsId",
                table: "Assignments",
                column: "SubjectDetailsId",
                principalTable: "SubjectDetails",
                principalColumn: "Id");
        }
    }
}
