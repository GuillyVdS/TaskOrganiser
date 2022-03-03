namespace TaskOrganiser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskToDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DueDate = c.DateTime(),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "StatusId", "dbo.Status");
            DropIndex("dbo.Tasks", new[] { "StatusId" });
            DropTable("dbo.Tasks");
        }
    }
}
