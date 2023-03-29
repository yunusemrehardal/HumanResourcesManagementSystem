using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class classpropfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateOfPackage",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "RemainingPackageTime",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "StartDateOfPackage",
                table: "Packages");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateOfPackage",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemainingPackageTime",
                table: "Companies",
                type: "nvarchar(48)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateOfPackage",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42f10cf4-273d-461a-a952-5eab02a5e30a",
                column: "ConcurrencyStamp",
                value: "abfcfd0c-3167-4f27-b442-0fcf0b51140d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e14a851-b637-4e8a-bb66-f7982895fa85",
                column: "ConcurrencyStamp",
                value: "c3fa1e3e-25aa-4204-ad3f-49e6ed220859");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edd822bf-ebb9-4772-989b-ecc423c705f2",
                column: "ConcurrencyStamp",
                value: "2050de08-42b4-448d-a67a-cc12260ef781");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b9ac8cc-9fd5-4a6d-b456-dafbf68ba98b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb5dcb0f-6133-411d-9c0c-2fc2263dea84", "AQAAAAEAACcQAAAAEM99LbUxCJThp1r5hqPKIDokv1s8SbhSQ/pTx2CbTjvfTJ7qUx+cVxSXho/LptCoFA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2eb6b745-c129-4244-a85b-225cd9f61ed2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b0d0b41d-0141-4d08-8e0b-4409e73b6199", "AQAAAAEAACcQAAAAELKUp/OVsMp+1vRWWaq0IGmyOZUJs2a5xim6haKjbueXXgxdsR27A0vzvrA9x+zaGQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80179962-814b-4ae3-aef3-a94cb0c8d01e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "277c86c8-a811-4efd-aa31-44656c032a05", "AQAAAAEAACcQAAAAENkrGah7wIF69yeP0kjP8lGq2M8o8lqWJrlYZfXYR778gAMtioD9Y9g6pFBACt7GXg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateOfPackage",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RemainingPackageTime",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "StartDateOfPackage",
                table: "Companies");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateOfPackage",
                table: "Packages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemainingPackageTime",
                table: "Packages",
                type: "nvarchar(48)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateOfPackage",
                table: "Packages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42f10cf4-273d-461a-a952-5eab02a5e30a",
                column: "ConcurrencyStamp",
                value: "4a9a7aaa-8ce8-4c60-8d59-9d87d80a5928");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e14a851-b637-4e8a-bb66-f7982895fa85",
                column: "ConcurrencyStamp",
                value: "ac299d23-30bd-457d-87c9-f1744fb91936");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edd822bf-ebb9-4772-989b-ecc423c705f2",
                column: "ConcurrencyStamp",
                value: "cb01d5c5-c1d0-4ddf-b026-0b251b554086");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b9ac8cc-9fd5-4a6d-b456-dafbf68ba98b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5c8c8606-7a99-4bde-b14a-43a3068816ee", "AQAAAAEAACcQAAAAELSW+U2bO2XwqB/yo882LJzSmqj4VtOLI2nDetv/fV5rs9jTFIWRF5O10JeVApWTyA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2eb6b745-c129-4244-a85b-225cd9f61ed2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4931efb6-dff8-4639-91c2-f3ddf56dee8b", "AQAAAAEAACcQAAAAEHzKBF35TqJuK25hH+n7C1zoHhDe+yb7t4TcA0tf7IXlSlI7cp/lsYE7MiBXVJiQzg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80179962-814b-4ae3-aef3-a94cb0c8d01e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c24a1f92-4469-4df0-bec1-73a41e36cee8", "AQAAAAEAACcQAAAAEFBVvqAyW1MOg47mg6WWR2aGpLroI53CQu9YD/YLXsrRje9+R6bKP3rQzTlpux63ug==" });
        }
    }
}
