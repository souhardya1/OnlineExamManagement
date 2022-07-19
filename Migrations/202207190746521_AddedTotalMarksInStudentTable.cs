namespace OnlineExamManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTotalMarksInStudentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Total", c => c.Int(nullable : false,defaultValue : 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Total");
        }
    }
}
