namespace DBRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecPos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpecialPositions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        ExecutorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ExecutorId, cascadeDelete: true)
                .Index(t => t.ExecutorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecialPositions", "ExecutorId", "dbo.Employees");
            DropIndex("dbo.SpecialPositions", new[] { "ExecutorId" });
            DropTable("dbo.SpecialPositions");
        }
    }
}
