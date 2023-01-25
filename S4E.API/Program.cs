using Microsoft.EntityFrameworkCore;
using S4E.Context;
using S4E.Profiles;
using S4E.Service;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<AssociadoService,AssociadoService>();
builder.Services.AddScoped<EmpresaService, EmpresaService>();


// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SQLServerDbContext>(options => {

    //"Data Source=DESKTOP-8KTP5PQ;Initial Catalog=S4E;Integrated Security=True";
    //Server=(localdb)\\mssqllocaldb;Database=S4E;Trusted_Connection=True;
    options.UseSqlServer(builder.Configuration.GetConnectionString("S4EDataBase"));
    options.UseSqlServer(b => b.MigrationsAssembly("S4E.API"));
});
builder.Services.AddAutoMapper(typeof(EmpresaProfile));
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
