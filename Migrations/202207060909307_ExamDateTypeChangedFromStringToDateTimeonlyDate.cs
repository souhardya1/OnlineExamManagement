namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamDateTypeChangedFromStringToDateTimeonlyDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exams", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exams", "Date", c => c.DateTime(nullable: false));
        }
    }
}
