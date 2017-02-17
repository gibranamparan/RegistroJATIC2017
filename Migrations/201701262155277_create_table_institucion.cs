namespace RegistroJATICS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table_institucion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Institucions",
                c => new
                    {
                        Nombre = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Nombre);
            
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String(nullable:false,maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ID_Taller", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Nombre");
            CreateIndex("dbo.AspNetUsers", "ID_Taller");
            AddForeignKey("dbo.AspNetUsers", "Nombre", "dbo.Institucions", "Nombre");
            AddForeignKey("dbo.AspNetUsers", "ID_Taller", "dbo.Tallers", "ID_Taller", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ID_Taller", "dbo.Tallers");
            DropForeignKey("dbo.AspNetUsers", "Nombre", "dbo.Institucions");
            DropIndex("dbo.AspNetUsers", new[] { "ID_Taller" });
            DropIndex("dbo.AspNetUsers", new[] { "Nombre" });
            DropColumn("dbo.AspNetUsers", "ID_Taller");
            DropColumn("dbo.AspNetUsers", "Nombre");
            DropTable("dbo.Institucions");
        }
    }
}
