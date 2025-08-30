using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.EF.Migrations
{
    /// <inheritdoc />
    public partial class modify_applicationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_RelationTypes_RelationTypeId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Specialties_SpecialtyId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SpecialtyId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Guardians_RelationTypeId",
                table: "Guardians");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SpecialtyId",
                table: "Teachers",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_RelationTypeId",
                table: "Guardians",
                column: "RelationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_RelationTypes_RelationTypeId",
                table: "Guardians",
                column: "RelationTypeId",
                principalTable: "RelationTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Specialties_SpecialtyId",
                table: "Teachers",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_RelationTypes_RelationTypeId",
                table: "Guardians");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Specialties_SpecialtyId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SpecialtyId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Guardians_RelationTypeId",
                table: "Guardians");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SpecialtyId",
                table: "Teachers",
                column: "SpecialtyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_RelationTypeId",
                table: "Guardians",
                column: "RelationTypeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_RelationTypes_RelationTypeId",
                table: "Guardians",
                column: "RelationTypeId",
                principalTable: "RelationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Specialties_SpecialtyId",
                table: "Teachers",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
