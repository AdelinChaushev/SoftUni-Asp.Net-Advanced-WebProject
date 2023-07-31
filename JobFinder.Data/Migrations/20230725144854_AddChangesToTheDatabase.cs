using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFinder.Data.Migrations
{
    public partial class AddChangesToTheDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications");

     

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("007a9393-c864-4134-bc4b-1de9bc46d10d"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("26751c0d-4a5b-401b-ad80-03f50f704ca9"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("4031bfe0-7468-482d-80de-8ea9ce5d4bf5"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("5ab07bf9-845e-499c-944e-66612ed7c2b7"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("315387ad-38bd-452e-99e9-de62844ff896"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("4fb60442-4486-4215-b7d7-a89b3ad8b58d"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("746334c1-0f7a-4ef7-88a8-a7477b61d593"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("8cf6f986-690b-4936-8fdf-880359510b69"));

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "JobCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7478976e-95c8-4475-8773-9bf08ea4acbc"), "Tech" },
                    { new Guid("7e0687c1-d6ac-4864-a820-c20a5728c41e"), "Farming" },
                    { new Guid("86ab0e84-44d0-4d0b-8c02-77114cbf1e3a"), "Finnance" },
                    { new Guid("87ef6adc-c066-4d68-99e6-5d833ba3bcf7"), "Architecture" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "WorkingSchedule" },
                values: new object[,]
                {
                    { new Guid("1f31c461-f893-4764-9c32-71171abf5697"), "9-5" },
                    { new Guid("21ff82a9-03e9-43ff-8466-ba8b71632c76"), "4 hours a day" },
                    { new Guid("69d34e85-5964-4691-8e60-559e784be3f8"), "full working week" },
                    { new Guid("a2c5e282-e2e5-47bf-a893-9cf2629baeaa"), "Weekends" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications");

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("7478976e-95c8-4475-8773-9bf08ea4acbc"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("7e0687c1-d6ac-4864-a820-c20a5728c41e"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("86ab0e84-44d0-4d0b-8c02-77114cbf1e3a"));

            migrationBuilder.DeleteData(
                table: "JobCategories",
                keyColumn: "Id",
                keyValue: new Guid("87ef6adc-c066-4d68-99e6-5d833ba3bcf7"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("1f31c461-f893-4764-9c32-71171abf5697"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("21ff82a9-03e9-43ff-8466-ba8b71632c76"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("69d34e85-5964-4691-8e60-559e784be3f8"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("a2c5e282-e2e5-47bf-a893-9cf2629baeaa"));

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Interviews");


            migrationBuilder.InsertData(
                table: "JobCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("007a9393-c864-4134-bc4b-1de9bc46d10d"), "Tech" },
                    { new Guid("26751c0d-4a5b-401b-ad80-03f50f704ca9"), "Farming" },
                    { new Guid("4031bfe0-7468-482d-80de-8ea9ce5d4bf5"), "Architecture" },
                    { new Guid("5ab07bf9-845e-499c-944e-66612ed7c2b7"), "Finnance" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "WorkingSchedule" },
                values: new object[,]
                {
                    { new Guid("315387ad-38bd-452e-99e9-de62844ff896"), "full working week" },
                    { new Guid("4fb60442-4486-4215-b7d7-a89b3ad8b58d"), "4 hours a day" },
                    { new Guid("746334c1-0f7a-4ef7-88a8-a7477b61d593"), "9-5" },
                    { new Guid("8cf6f986-690b-4936-8fdf-880359510b69"), "Weekends" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CompanyId",
                table: "Reviews",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
