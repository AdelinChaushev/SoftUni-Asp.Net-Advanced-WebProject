using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFinder.Data.Migrations
{
    public partial class SeedingJobCategoriesAndSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "JobCategories");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CategoryDescription",
                table: "JobCategories",
                type: "nvarchar(270)",
                maxLength: 270,
                nullable: false,
                defaultValue: "");
        }
    }
}
