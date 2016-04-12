namespace AuthSln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OracleMig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "USERPRUEBA.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "USERPRUEBA.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("USERPRUEBA.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("USERPRUEBA.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "USERPRUEBA.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TwoFactorEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AccessFailedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "USERPRUEBA.AspNetUserClaims",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("USERPRUEBA.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "USERPRUEBA.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("USERPRUEBA.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("USERPRUEBA.AspNetUserRoles", "UserId", "USERPRUEBA.AspNetUsers");
            DropForeignKey("USERPRUEBA.AspNetUserLogins", "UserId", "USERPRUEBA.AspNetUsers");
            DropForeignKey("USERPRUEBA.AspNetUserClaims", "UserId", "USERPRUEBA.AspNetUsers");
            DropForeignKey("USERPRUEBA.AspNetUserRoles", "RoleId", "USERPRUEBA.AspNetRoles");
            DropIndex("USERPRUEBA.AspNetUserLogins", new[] { "UserId" });
            DropIndex("USERPRUEBA.AspNetUserClaims", new[] { "UserId" });
            DropIndex("USERPRUEBA.AspNetUsers", "UserNameIndex");
            DropIndex("USERPRUEBA.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("USERPRUEBA.AspNetUserRoles", new[] { "UserId" });
            DropIndex("USERPRUEBA.AspNetRoles", "RoleNameIndex");
            DropTable("USERPRUEBA.AspNetUserLogins");
            DropTable("USERPRUEBA.AspNetUserClaims");
            DropTable("USERPRUEBA.AspNetUsers");
            DropTable("USERPRUEBA.AspNetUserRoles");
            DropTable("USERPRUEBA.AspNetRoles");
        }
    }
}
