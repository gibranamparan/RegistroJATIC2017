namespace RegistroJATICS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_table_talleres2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Taller2",
                c => new
                    {
                        ID_Taller2 = c.Int(nullable: false, identity: true),
                        Nombre_Taller = c.String(nullable: false),
                        Descripcion = c.String(),
                        CantidadParticipantes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_Taller2);
            
            AddColumn("dbo.AspNetUsers", "ID_Taller2", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ID_Taller2");
            AddForeignKey("dbo.AspNetUsers", "ID_Taller2", "dbo.Taller2", "ID_Taller2");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ID_Taller2", "dbo.Taller2");
            DropIndex("dbo.AspNetUsers", new[] { "ID_Taller2" });
            DropColumn("dbo.AspNetUsers", "ID_Taller2");
            DropTable("dbo.Taller2");
        }
    }
}
