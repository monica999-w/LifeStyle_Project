using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeStyle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Nutrients_NutrientsNutrientId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Planners_UserProfiles_ProfileId",
                table: "Planners");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Planners",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NutrientsNutrientId",
                table: "Meals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Nutrients_NutrientsNutrientId",
                table: "Meals",
                column: "NutrientsNutrientId",
                principalTable: "Nutrients",
                principalColumn: "NutrientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planners_UserProfiles_ProfileId",
                table: "Planners",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Nutrients_NutrientsNutrientId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Planners_UserProfiles_ProfileId",
                table: "Planners");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Planners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NutrientsNutrientId",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Nutrients_NutrientsNutrientId",
                table: "Meals",
                column: "NutrientsNutrientId",
                principalTable: "Nutrients",
                principalColumn: "NutrientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Planners_UserProfiles_ProfileId",
                table: "Planners",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
