using Autofac;
using HotelBookingApp.Data;
using HotelBookingApp.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.AppModules
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Registrera ApplicationDbContext
            builder.Register(context =>
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>();
                options.UseSqlServer(@"Server=.;Database=HotelAppDb;Trusted_Connection=True;TrustServerCertificate=true;");
                return new ApplicationDbContext(options.Options);
            }).As<ApplicationDbContext>().InstancePerLifetimeScope();

            // Registrera DataInitializer
            builder.RegisterType<DataInitializer>().AsSelf().SingleInstance();

            // Registrera andra tjänster och repositories
            //builder.RegisterType<NavigationMenu>().AsSelf().SingleInstance();
            //builder.RegisterType<RoomService>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<CustomerService>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<BookingService>().AsSelf().InstancePerLifetimeScope();
            ////builder.RegisterType<InvoiceService>().AsSelf().InstancePerLifetimeScope();

            //builder.RegisterType<RoomRepository>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<CustomerRepository>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<BookingRepository>().AsSelf().InstancePerLifetimeScope();
            ////builder.RegisterType<InvoiceRepository>().AsSelf().InstancePerLifetimeScope();

            //builder.RegisterType<CustomerController>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<BookingController>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<RoomController>().AsSelf().InstancePerLifetimeScope();

            //builder.RegisterType<MenuManager>().AsSelf().InstancePerLifetimeScope();
        }


    }
}
