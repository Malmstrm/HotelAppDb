using Autofac;
using HotelAppDb.Service;
using HotelAppDb.Repositories;
using HotelAppDb.Interfaces;
using HotelAppDb.Data;
using HotelAppDb.Controllers;
using Microsoft.EntityFrameworkCore;

namespace HotelAppDb.AppModules
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

            //Registrera andra tjänster och repositories
            builder.RegisterType<NavigationMenu>().AsSelf().SingleInstance();
            builder.RegisterType<RoomService>().As<IRoomService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<BookingService>().As<IBookingService>().InstancePerLifetimeScope();
            //builder.RegisterType<InvoiceService>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<RoomRepository>().As<IRoomRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BookingRepository>().As<IBookingRepository>().InstancePerLifetimeScope();
            //builder.RegisterType<InvoiceRepository>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<CustomerController>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<BookingController>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RoomController>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<MainMenu>().AsSelf().InstancePerLifetimeScope();
        }


    }
}
