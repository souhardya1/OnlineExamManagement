namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEamStudentDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        ExamCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExamStudents");
        }
    }
}
