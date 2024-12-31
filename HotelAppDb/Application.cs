using Autofac;
using HotelAppDb.Data;
using HotelAppDb.AppModules;
using HotelAppDb.Controllers;


namespace HotelBookingApp
{
    public class Application
    {
        private readonly IContainer _container;
        public Application()
        {
            // Konfigurera Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule<AppModule>();
            _container = builder.Build();
        }
        public void Run()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                // Hämta DataInitializer och kör migration och seed
                var initializer = scope.Resolve<DataInitializer>();
                var dbContext = scope.Resolve<ApplicationDbContext>();
                initializer.MigrateAndSeed(dbContext);

                // Hämta MenuManager från Autofac och starta menyerna

                var mainMenu = scope.Resolve<MainMenu>();
                mainMenu.Run();
            }
        }
    }
}
