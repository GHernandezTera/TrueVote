using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrueVote.Migrations
{
    /// <inheritdoc />
    public partial class regionCodesAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_States_StateId",
                table: "Municipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Parishes_Municipalities_MunicipalityId",
                table: "Parishes");

            migrationBuilder.DropColumn(
                name: "Municipality",
                table: "VotingRecords");

            migrationBuilder.DropColumn(
                name: "Parish",
                table: "VotingRecords");

            migrationBuilder.DropColumn(
                name: "State",
                table: "VotingRecords");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "VotingRecords",
                newName: "StateCode");

            migrationBuilder.RenameColumn(
                name: "ParishId",
                table: "VotingRecords",
                newName: "ParishCode");

            migrationBuilder.RenameColumn(
                name: "MunicipalityId",
                table: "VotingRecords",
                newName: "MunicipalityCode");

            migrationBuilder.RenameColumn(
                name: "MunicipalityId",
                table: "Parishes",
                newName: "MunicipalityCode");

            migrationBuilder.RenameIndex(
                name: "IX_Parishes_MunicipalityId",
                table: "Parishes",
                newName: "IX_Parishes_MunicipalityCode");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Municipalities",
                newName: "StateCode");

            migrationBuilder.RenameIndex(
                name: "IX_Municipalities_StateId",
                table: "Municipalities",
                newName: "IX_Municipalities_StateCode");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "States",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Parishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Municipalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_States_Code",
                table: "States",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Parishes_Code",
                table: "Parishes",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Municipalities_Code",
                table: "Municipalities",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_VotingRecords_MunicipalityCode",
                table: "VotingRecords",
                column: "MunicipalityCode");

            migrationBuilder.CreateIndex(
                name: "IX_VotingRecords_ParishCode",
                table: "VotingRecords",
                column: "ParishCode");

            migrationBuilder.CreateIndex(
                name: "IX_VotingRecords_StateCode",
                table: "VotingRecords",
                column: "StateCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_States_StateCode",
                table: "Municipalities",
                column: "StateCode",
                principalTable: "States",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parishes_Municipalities_MunicipalityCode",
                table: "Parishes",
                column: "MunicipalityCode",
                principalTable: "Municipalities",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingRecords_Municipalities_MunicipalityCode",
                table: "VotingRecords",
                column: "MunicipalityCode",
                principalTable: "Municipalities",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingRecords_Parishes_ParishCode",
                table: "VotingRecords",
                column: "ParishCode",
                principalTable: "Parishes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingRecords_States_StateCode",
                table: "VotingRecords",
                column: "StateCode",
                principalTable: "States",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_States_StateCode",
                table: "Municipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Parishes_Municipalities_MunicipalityCode",
                table: "Parishes");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingRecords_Municipalities_MunicipalityCode",
                table: "VotingRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingRecords_Parishes_ParishCode",
                table: "VotingRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_VotingRecords_States_StateCode",
                table: "VotingRecords");

            migrationBuilder.DropIndex(
                name: "IX_VotingRecords_MunicipalityCode",
                table: "VotingRecords");

            migrationBuilder.DropIndex(
                name: "IX_VotingRecords_ParishCode",
                table: "VotingRecords");

            migrationBuilder.DropIndex(
                name: "IX_VotingRecords_StateCode",
                table: "VotingRecords");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_States_Code",
                table: "States");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Parishes_Code",
                table: "Parishes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Municipalities_Code",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "States");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Parishes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Municipalities");

            migrationBuilder.RenameColumn(
                name: "StateCode",
                table: "VotingRecords",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "ParishCode",
                table: "VotingRecords",
                newName: "ParishId");

            migrationBuilder.RenameColumn(
                name: "MunicipalityCode",
                table: "VotingRecords",
                newName: "MunicipalityId");

            migrationBuilder.RenameColumn(
                name: "MunicipalityCode",
                table: "Parishes",
                newName: "MunicipalityId");

            migrationBuilder.RenameIndex(
                name: "IX_Parishes_MunicipalityCode",
                table: "Parishes",
                newName: "IX_Parishes_MunicipalityId");

            migrationBuilder.RenameColumn(
                name: "StateCode",
                table: "Municipalities",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Municipalities_StateCode",
                table: "Municipalities",
                newName: "IX_Municipalities_StateId");

            migrationBuilder.AddColumn<string>(
                name: "Municipality",
                table: "VotingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Parish",
                table: "VotingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "VotingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_States_StateId",
                table: "Municipalities",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parishes_Municipalities_MunicipalityId",
                table: "Parishes",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
