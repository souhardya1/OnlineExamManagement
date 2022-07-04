namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Date = c.String(nullable: false),
                        Active = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Marks1 = c.Int(),
                        Marks2 = c.Int(),
                        Marks3 = c.Int(),
                        Marks4 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ques = c.String(nullable: false, maxLength: 100),
                        OptionA = c.String(nullable: false, maxLength: 100),
                        OptionB = c.String(nullable: false, maxLength: 100),
                        OptionC = c.String(nullable: false, maxLength: 100),
                        OptionD = c.String(nullable: false, maxLength: 100),
                        Correct = c.String(nullable: false, maxLength: 100),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Designation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentExams",
                c => new
                    {
                        Student_Id = c.Int(nullable: false),
                        Exam_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Exam_Id })
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Exam_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.StudentExams", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.StudentExams", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Exams", "Course_Id", "dbo.Courses");
            DropIndex("dbo.StudentExams", new[] { "Exam_Id" });
            DropIndex("dbo.StudentExams", new[] { "Student_Id" });
            DropIndex("dbo.Questions", new[] { "Course_Id" });
            DropIndex("dbo.Exams", new[] { "Course_Id" });
            DropTable("dbo.StudentExams");
            DropTable("dbo.Teachers");
            DropTable("dbo.Questions");
            DropTable("dbo.Students");
            DropTable("dbo.Exams");
            DropTable("dbo.Courses");
        }
    }
}
