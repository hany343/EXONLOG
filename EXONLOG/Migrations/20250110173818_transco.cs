using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXONLOG.Migrations
{
    /// <inheritdoc />
    public partial class transco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Importers_ImporterID",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ImporterID",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ImporterID",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "ImporterID",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "WeighNotes",
                table: "OutLadings",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "StackBar",
                table: "OutLadings",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FillType",
                table: "OutLadings",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TransCompanyID",
                table: "InLadings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ImporterID",
                table: "Shipments",
                column: "ImporterID");

            migrationBuilder.CreateIndex(
                name: "IX_OutLadings_TransCompanyID",
                table: "OutLadings",
                column: "TransCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_InLadings_TransCompanyID",
                table: "InLadings",
                column: "TransCompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_InLadings_TransCompanies_TransCompanyID",
                table: "InLadings",
                column: "TransCompanyID",
                principalTable: "TransCompanies",
                principalColumn: "TransCompanyID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutLadings_TransCompanies_TransCompanyID",
                table: "OutLadings",
                column: "TransCompanyID",
                principalTable: "TransCompanies",
                principalColumn: "TransCompanyID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Importers_ImporterID",
                table: "Shipments",
                column: "ImporterID",
                principalTable: "Importers",
                principalColumn: "ImporterID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InLadings_TransCompanies_TransCompanyID",
                table: "InLadings");

            migrationBuilder.DropForeignKey(
                name: "FK_OutLadings_TransCompanies_TransCompanyID",
                table: "OutLadings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Importers_ImporterID",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ImporterID",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_OutLadings_TransCompanyID",
                table: "OutLadings");

            migrationBuilder.DropIndex(
                name: "IX_InLadings_TransCompanyID",
                table: "InLadings");

            migrationBuilder.DropColumn(
                name: "ImporterID",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "TransCompanyID",
                table: "InLadings");

            migrationBuilder.AlterColumn<string>(
                name: "WeighNotes",
                table: "OutLadings",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StackBar",
                table: "OutLadings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "FillType",
                table: "OutLadings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AddColumn<int>(
                name: "ImporterID",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ImporterID",
                table: "Contracts",
                column: "ImporterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Importers_ImporterID",
                table: "Contracts",
                column: "ImporterID",
                principalTable: "Importers",
                principalColumn: "ImporterID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
