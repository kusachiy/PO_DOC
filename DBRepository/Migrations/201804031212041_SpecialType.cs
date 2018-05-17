namespace DBRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecialType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disciplines", "SpecialType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Disciplines", "SpecialType");
        }
    }
}
