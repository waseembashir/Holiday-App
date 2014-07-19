using System.Data.Entity.Migrations;
using HolidayApp.Core.Data;

namespace Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HolidayAppDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        //protected override void Seed(ClientDb db)
        //{

        //}

    }
}
