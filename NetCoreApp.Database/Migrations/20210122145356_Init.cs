using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreApp.Database.Migrations
{
  public partial class Init : Migration
  {
    protected override void Up( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new
          {
            Id = table.Column<string>( nullable: false ),
            Version = table.Column<int>( nullable: false ),
            CreatedTime = table.Column<DateTime>( nullable: false ),
            LastModifiedTime = table.Column<DateTime>( nullable: false ),
            IsDeleted = table.Column<bool>( nullable: false ),
            Username = table.Column<string>( nullable: true ),
            Name = table.Column<string>( nullable: true ),
            Email = table.Column<string>( nullable: true )
          },
          constraints: table =>
          {
            table.PrimaryKey( "PK_Users", x => x.Id );
          } );
    }

    protected override void Down( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.DropTable(
          name: "Users" );
    }
  }
}
