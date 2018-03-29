namespace DBRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodeNumber = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        DepartmentId = c.Guid(nullable: false),
                        IsSpecial = c.Boolean(nullable: false),
                        SpecialType = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.DisciplineYears",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisciplineId = c.Guid(nullable: false),
                        CountOfLecture = c.Int(nullable: false),
                        CountOfPractice = c.Int(nullable: false),
                        CountOfLabs = c.Int(nullable: false),
                        HasKZ = c.Boolean(nullable: false),
                        HasKR = c.Boolean(nullable: false),
                        HasKP = c.Boolean(nullable: false),
                        HasEx = c.Boolean(nullable: false),
                        HasCR = c.Boolean(nullable: false),
                        HasCons = c.Boolean(nullable: false),
                        SpecialParameter = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        AcademicDegree = c.Int(nullable: false),
                        AcademicRank = c.Int(nullable: false),
                        WorkingPosition = c.Int(nullable: false),
                        Rate = c.Single(nullable: false),
                        ContractNumber = c.Int(nullable: false),
                        BirthDay = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ShortName = c.String(unicode: false),
                        FullName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        Qualification = c.Int(nullable: false),
                        StudyForm = c.Int(nullable: false),
                        SpecialityId = c.Guid(nullable: false),
                        CountOfStudents = c.Int(nullable: false),
                        EntryYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialities", t => t.SpecialityId, cascadeDelete: true)
                .Index(t => t.SpecialityId);
            
            CreateTable(
                "dbo.Specialities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        FacultyId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Faculties", t => t.FacultyId, cascadeDelete: true)
                .Index(t => t.FacultyId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        GroupId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.DisciplineWorkloads",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisciplineYearId = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        SemesterId = c.Guid(nullable: false),
                        StudyYearId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisciplineYears", t => t.DisciplineYearId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Semesters", t => t.SemesterId, cascadeDelete: true)
                .ForeignKey("dbo.StudyYears", t => t.StudyYearId, cascadeDelete: true)
                .Index(t => t.DisciplineYearId)
                .Index(t => t.GroupId)
                .Index(t => t.SemesterId)
                .Index(t => t.StudyYearId);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Number = c.Int(nullable: false),
                        CountOfWeeks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudyYears",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workloads",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LocalWorkloadId = c.Guid(nullable: false),
                        EmployeeId = c.Guid(),
                        CountOfStudents = c.Int(nullable: false),
                        CountOfWeeks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.DisciplineWorkloads", t => t.LocalWorkloadId, cascadeDelete: true)
                .Index(t => t.LocalWorkloadId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workloads", "LocalWorkloadId", "dbo.DisciplineWorkloads");
            DropForeignKey("dbo.Workloads", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.DisciplineWorkloads", "StudyYearId", "dbo.StudyYears");
            DropForeignKey("dbo.DisciplineWorkloads", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.DisciplineWorkloads", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.DisciplineWorkloads", "DisciplineYearId", "dbo.DisciplineYears");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.Specialities", "FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.DisciplineYears", "DisciplineId", "dbo.Disciplines");
            DropForeignKey("dbo.Disciplines", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Workloads", new[] { "EmployeeId" });
            DropIndex("dbo.Workloads", new[] { "LocalWorkloadId" });
            DropIndex("dbo.DisciplineWorkloads", new[] { "StudyYearId" });
            DropIndex("dbo.DisciplineWorkloads", new[] { "SemesterId" });
            DropIndex("dbo.DisciplineWorkloads", new[] { "GroupId" });
            DropIndex("dbo.DisciplineWorkloads", new[] { "DisciplineYearId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Specialities", new[] { "FacultyId" });
            DropIndex("dbo.Groups", new[] { "SpecialityId" });
            DropIndex("dbo.DisciplineYears", new[] { "DisciplineId" });
            DropIndex("dbo.Disciplines", new[] { "DepartmentId" });
            DropTable("dbo.Workloads");
            DropTable("dbo.StudyYears");
            DropTable("dbo.Semesters");
            DropTable("dbo.DisciplineWorkloads");
            DropTable("dbo.Students");
            DropTable("dbo.Specialities");
            DropTable("dbo.Groups");
            DropTable("dbo.Faculties");
            DropTable("dbo.Employees");
            DropTable("dbo.DisciplineYears");
            DropTable("dbo.Disciplines");
            DropTable("dbo.Departments");
        }
    }
}
