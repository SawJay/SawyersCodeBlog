using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SawyersCodeBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class Schematize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blog");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Images",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comments",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "BlogPostTag",
                newName: "BlogPostTag",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "BlogPosts",
                newName: "BlogPosts",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "blog");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "blog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "blog",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Images",
                schema: "blog",
                newName: "Images");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "blog",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "blog",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "BlogPostTag",
                schema: "blog",
                newName: "BlogPostTag");

            migrationBuilder.RenameTable(
                name: "BlogPosts",
                schema: "blog",
                newName: "BlogPosts");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "blog",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "blog",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "blog",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "blog",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "blog",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "blog",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "blog",
                newName: "AspNetRoleClaims");
        }
    }
}
