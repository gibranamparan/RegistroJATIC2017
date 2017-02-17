namespace RegistroJATICS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnCantidadPArticipantesToTaller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tallers", "CantidadParticipantes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tallers", "CantidadParticipantes");
        }
    }
}
