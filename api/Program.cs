using Microsoft.EntityFrameworkCore;
using api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("SchoolContext")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   try
   {
       // add 10 seconds delay to ensure the db server is up to accept connections
       // this won't be needed in real world application
       System.Threading.Thread.Sleep(10000);
       var context = services.GetRequiredService<AppDbContext>();
       var created = context.Database.EnsureCreated();

   }
   catch (Exception ex)
   {
       var logger = services.GetRequiredService<ILogger<Program>>();
       logger.LogError(ex, "An error occurred creating the DB.");
   }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
