namespace AspIdentityWebAPI.Migrations
{
    using Infraestructura;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AspIdentityWebAPI.Infraestructura.AppDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AppDBContext()));
            var user = new IdentityUser()
            {
                UserName = "SuperPowerUser",
                Email = "correobasuraregistro@gmail.com",
                EmailConfirmed = true
            };
            manager.Create(user, "MySuperP@ssword!");
        }
    }
}
