namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDelinquencyToCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "DelinquentOnPayment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "DelinquentOnPayment");
        }
    }
}
