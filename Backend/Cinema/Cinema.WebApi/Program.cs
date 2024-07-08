using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Cinema.Mapper;
using Cinema.Repository;
using Cinema.Repository.Common;
using Cinema.Service;
using Cinema.Service.Common;
using Cinema.WebApi.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
string connectionString = builder.Configuration.GetSection("AppSettings").GetValue<String>("ConnectionString");

builder.Services.AddAutoMapper(typeof(UserProfile), typeof(ReviewProfile), typeof(MovieProfile), typeof(TicketProfile), typeof(ProjectionProfile));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder
        .RegisterType<MovieService>()
        .As<IMovieService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<MovieRepository>()
        .As<IMovieRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<TicketService>()
        .As<ITicketService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<TicketRepository>()
        .As<ITicketRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ProjectionService>()
        .As<IProjectionService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ProjectionRepository>()
        .As<IProjectionRepository>()
        .InstancePerLifetimeScope();
    containerBuilder
        .RegisterType<UserService>()
        .As<IUserService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<SeatReservedService>()
        .As<ISeatReservedService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<SeatService>()
        .As<ISeatService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<SeatRepository>()
        .As<ISeatRepository>()
        .InstancePerLifetimeScope();
    containerBuilder
        .RegisterType<UserRepository>()
        .As<IUserRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ActorService>()
        .As<IActorService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ActorRepository>()
        .As<IActorRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<HallService>()
        .As<IHallService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<HallRepository>()
        .As<IHallRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ReviewService>()
        .As<IReviewService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<ReviewRepository>()
        .As<IReviewRepository>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<PaymentService>()
        .As<IPaymentService>()
        .InstancePerLifetimeScope();
    
    containerBuilder
        .RegisterType<PaymentRepository>()
        .As<IPaymentRepository>()
        .InstancePerLifetimeScope();
    
    containerBuilder
        .RegisterType<GenreService>()
        .As<IGenreService>()
        .InstancePerLifetimeScope();
    
    containerBuilder
        .RegisterType<GenreRepository>()
        .As<IGenreRepository>()
        .InstancePerLifetimeScope();
    
    containerBuilder
        .RegisterType<LanguageService>()
        .As<ILanguageService>()
        .InstancePerLifetimeScope();
    
    containerBuilder
        .RegisterType<LanguageRepository>()
        .As<ILanguageRepository>()
        .InstancePerLifetimeScope();
    
    

    containerBuilder.RegisterInstance(connectionString).As<string>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.Run();