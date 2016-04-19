namespace AspIdentityWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "USERIDENTITY.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "USERIDENTITY.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("USERIDENTITY.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("USERIDENTITY.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "USERIDENTITY.AspNetUsers",
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
                "USERIDENTITY.AspNetUserClaims",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("USERIDENTITY.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "USERIDENTITY.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("USERIDENTITY.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("USERIDENTITY.AspNetUserRoles", "UserId", "USERIDENTITY.AspNetUsers");
            DropForeignKey("USERIDENTITY.AspNetUserLogins", "UserId", "USERIDENTITY.AspNetUsers");
            DropForeignKey("USERIDENTITY.AspNetUserClaims", "UserId", "USERIDENTITY.AspNetUsers");
            DropForeignKey("USERIDENTITY.AspNetUserRoles", "RoleId", "USERIDENTITY.AspNetRoles");
            DropIndex("USERIDENTITY.AspNetUserLogins", new[] { "UserId" });
            DropIndex("USERIDENTITY.AspNetUserClaims", new[] { "UserId" });
            DropIndex("USERIDENTITY.AspNetUsers", "UserNameIndex");
            DropIndex("USERIDENTITY.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("USERIDENTITY.AspNetUserRoles", new[] { "UserId" });
            DropIndex("USERIDENTITY.AspNetRoles", "RoleNameIndex");
            DropTable("USERIDENTITY.AspNetUserLogins");
            DropTable("USERIDENTITY.AspNetUserClaims");
            DropTable("USERIDENTITY.AspNetUsers");
            DropTable("USERIDENTITY.AspNetUserRoles");
            DropTable("USERIDENTITY.AspNetRoles");
        }
    }
}
