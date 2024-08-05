using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal_Diary.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "TaskTypes",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Color = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TaskTypes", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Tasks",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					TaskTitle = table.Column<string>(type: "TEXT", nullable: false),
					StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
					EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
					Description = table.Column<string>(type: "TEXT", nullable: true),
					TaskTypeId = table.Column<int>(type: "INTEGER", nullable: false),
					Frequency = table.Column<int>(type: "INTEGER", nullable: false),
					Status = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tasks", x => x.Id);
					table.ForeignKey(
						name: "FK_Tasks_TaskTypes_TaskTypeId",
						column: x => x.TaskTypeId,
						principalTable: "TaskTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Tasks_TaskTypeId",
				table: "Tasks",
				column: "TaskTypeId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Tasks");

			migrationBuilder.DropTable(
				name: "TaskTypes");
		}
	}
}
