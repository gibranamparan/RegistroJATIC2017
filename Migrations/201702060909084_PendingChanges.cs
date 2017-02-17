namespace RegistroJATICS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tallers", "Nombre_Taller", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tallers", "Nombre_Taller", c => c.String());
        }
    }
}
