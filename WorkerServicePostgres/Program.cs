using Microsoft.EntityFrameworkCore;
using WorkerServicePostgres.Data;
using WorkerServicePostgres.Entities;
using WorkerServicePostgres.Repositories;
using WorkerServicePostgres.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<MyDbContext>(provider =>
{
    var options = new DbContextOptionsBuilder<MyDbContext>()
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
        .Options;
    return new MyDbContext(options);
});

builder.Services.AddSingleton<IRepository<Carro>, CarroRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();