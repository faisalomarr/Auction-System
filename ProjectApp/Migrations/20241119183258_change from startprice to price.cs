using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Migrations
{
    /// <inheritdoc />
    public partial class changefromstartpricetoprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the 'Price' column to replace 'StartPrice'
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AuctionsDbs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // Migrate data from 'StartPrice' to 'Price'
            migrationBuilder.Sql("UPDATE AuctionsDbs SET Price = StartPrice");

            // Drop the 'StartPrice' column
            migrationBuilder.DropColumn(
                name: "StartPrice",
                table: "AuctionsDbs");

            // Rename the 'username' column to 'Username' if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'AuctionsDbs' AND COLUMN_NAME = 'username'
                )
                THEN
                    ALTER TABLE AuctionsDbs CHANGE `username` `Username` VARCHAR(100) NOT NULL;
                END IF;
            ");

            // Rename the 'BidId' column to 'Id'
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'BidDbs' AND COLUMN_NAME = 'BidId'
                )
                THEN
                    ALTER TABLE BidDbs CHANGE `BidId` `Id` INT NOT NULL;
                END IF;
            ");

            // Alter the 'Description' column type
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AuctionsDbs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the 'Price' column back to 'StartPrice'
            migrationBuilder.AddColumn<decimal>(
                name: "StartPrice",
                table: "AuctionsDbs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // Migrate data back from 'Price' to 'StartPrice'
            migrationBuilder.Sql("UPDATE AuctionsDbs SET StartPrice = Price");

            // Drop the 'Price' column
            migrationBuilder.DropColumn(
                name: "Price",
                table: "AuctionsDbs");

            // Rename the 'Username' column back to 'username' if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'AuctionsDbs' AND COLUMN_NAME = 'Username'
                )
                THEN
                    ALTER TABLE AuctionsDbs CHANGE `Username` `username` VARCHAR(100) NOT NULL;
                END IF;
            ");

            // Rename the 'Id' column back to 'BidId'
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'BidDbs' AND COLUMN_NAME = 'Id'
                )
                THEN
                    ALTER TABLE BidDbs CHANGE `Id` `BidId` INT NOT NULL;
                END IF;
            ");

            // Revert the 'Description' column type
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AuctionsDbs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);
        }
    }
}
