namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCourserefidToQuestionsTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Questions", name: "Course_Id", newName: "CourseRefId");
            RenameIndex(table: "dbo.Questions", name: "IX_Course_Id", newName: "IX_CourseRefId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Questions", name: "IX_CourseRefId", newName: "IX_Course_Id");
            RenameColumn(table: "dbo.Questions", name: "CourseRefId", newName: "Course_Id");
        }
    }
}
