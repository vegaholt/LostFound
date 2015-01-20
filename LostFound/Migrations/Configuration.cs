namespace LostFound.Migrations
{
    using LostFound.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LostFound.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LostFound.Models.ApplicationDbContext context)
        {
            context.Categories.AddOrUpdate(i => i.Name,
                new Category { Name = "Annet", ImageUrl = "/Content/Img/Annet.png" },
                new Category { Name = "Bagger/Sekker", ImageUrl = "/Content/Img/Bagasje.png" },
                new Category { Name = "Briller", ImageUrl = "/Content/Img/Briller.png" },
                new Category { Name = "Datamaskiner", ImageUrl = "/Content/Img/Data.png" },
                new Category { Name = "Kj�ledyr", ImageUrl = "/Content/Img/Dyr.png" },
                new Category { Name = "Hansker", ImageUrl = "/Content/Img/Hansker.png" },
                new Category { Name = "Kl�r", ImageUrl = "/Content/Img/Klaer.png" },
                new Category { Name = "Lommeb�ker/kort", ImageUrl = "/Content/Img/Kort.png" },
                new Category { Name = "N�kler", ImageUrl = "/Content/Img/Nokkel.png" },
                new Category { Name = "Sykler", ImageUrl = "/Content/Img/Sykkel.png" },
                new Category { Name = "Mobiler/Nettbrett", ImageUrl = "/Content/Img/Telefon.png" }
            );
            context.SaveChanges();

            context.Counties.AddOrUpdate(i => i.Name,
                new County { Name = "Akershus" },
                new County { Name = "Aust-Agder" },
                new County { Name = "Buskerud" },
                new County { Name = "Finmark" },
                new County { Name = "Hedmark" },
                new County { Name = "Hordaland" },
                new County { Name = "M�re og Romsdal" },
                new County { Name = "Nordland" },
                new County { Name = "Nord-Tr�ndelag" },
                new County { Name = "Oppland" },
                new County { Name = "Oslo" },
                new County { Name = "Rogaland" },
                new County { Name = "Sogn og Fjordane" },
                new County { Name = "S�r-Tr�ndelag" },
                new County { Name = "Telemark" },
                new County { Name = "Troms" },
                new County { Name = "Vest-Agder" },
                new County { Name = "Vestfold" },
                new County { Name = "�stfold" }
            );
            context.SaveChanges();
        }
    }
}
