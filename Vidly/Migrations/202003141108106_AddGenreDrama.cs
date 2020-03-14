namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenreDrama : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[Genres] ([Id], [Name]) VALUES (6, N'Drama')");
        }
        
        public override void Down()
        {
            Sql("delete [dbo].[Genres] where Id = 6");
        }
    }
}
