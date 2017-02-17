namespace RegistroJATICS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingchanges2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "NombreAsistente");
            AddColumn("dbo.AspNetUsers", "NombreCompleto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NombreCompleto");
        }
    }
}
