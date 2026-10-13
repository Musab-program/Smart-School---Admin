using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeederAndSubmittedAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAssignment_Assignments_AssignmentId",
                table: "SubmittedAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAssignment_Students_StudentId",
                table: "SubmittedAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmittedAssignment",
                table: "SubmittedAssignment");

            migrationBuilder.RenameTable(
                name: "SubmittedAssignment",
                newName: "SubmittedAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedAssignment_StudentId_AssignmentId",
                table: "SubmittedAssignments",
                newName: "IX_SubmittedAssignments_StudentId_AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedAssignment_AssignmentId",
                table: "SubmittedAssignments",
                newName: "IX_SubmittedAssignments_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmittedAssignments",
                table: "SubmittedAssignments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAssignments_Assignments_AssignmentId",
                table: "SubmittedAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAssignments_Students_StudentId",
                table: "SubmittedAssignments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAssignments_Assignments_AssignmentId",
                table: "SubmittedAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAssignments_Students_StudentId",
                table: "SubmittedAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmittedAssignments",
                table: "SubmittedAssignments");

            migrationBuilder.RenameTable(
                name: "SubmittedAssignments",
                newName: "SubmittedAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedAssignments_StudentId_AssignmentId",
                table: "SubmittedAssignment",
                newName: "IX_SubmittedAssignment_StudentId_AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedAssignments_AssignmentId",
                table: "SubmittedAssignment",
                newName: "IX_SubmittedAssignment_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmittedAssignment",
                table: "SubmittedAssignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAssignment_Assignments_AssignmentId",
                table: "SubmittedAssignment",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAssignment_Students_StudentId",
                table: "SubmittedAssignment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
