namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamDateTypeChangedFromStringToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exams", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exams", "Date", c => c.String(nullable: false));
        }
    }
}
