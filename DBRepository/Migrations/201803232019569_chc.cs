namespace DBRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chc : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Disciplines", "SpecialType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Disciplines", "SpecialType", c => c.Int());
        }
    }
}
