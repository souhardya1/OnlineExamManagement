namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCourseRefIdToExamtable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Exams", name: "Course_Id", newName: "CourseRefId");
            RenameIndex(table: "dbo.Exams", name: "IX_Course_Id", newName: "IX_CourseRefId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Exams", name: "IX_CourseRefId", newName: "IX_Course_Id");
            RenameColumn(table: "dbo.Exams", name: "CourseRefId", newName: "Course_Id");
        }
    }
}
