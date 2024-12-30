using Autofac;
using HotelBookingApp.AppModules;
using HotelBookingApp.Data;


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

                //var menuManager = scope.Resolve<MenuManager>();
                //menuManager.Run();
            }
        }
    }
}
