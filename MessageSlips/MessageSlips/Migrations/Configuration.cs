using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using MessageSlips.Models;



namespace MessageSlips.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MessageSlips.Models.MessageSlipsWSGEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        bool AddUser(MessageSlips.Models.MessageSlipsWSGEntities context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("admin"));
            //var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            return true;
        }

        protected override void Seed(MessageSlips.Models.MessageSlipsWSGEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
