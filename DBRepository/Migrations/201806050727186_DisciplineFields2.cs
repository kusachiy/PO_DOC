namespace DBRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisciplineFields2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disciplines", "CountOfWorkloads", c => c.Int(nullable: false));
            AddColumn("dbo.Disciplines", "IsActiveDiscipline", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Disciplines", "IsActiveDiscipline");
            DropColumn("dbo.Disciplines", "CountOfWorkloads");
        }
    }
}
