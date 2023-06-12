using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using api.Data;
using HealthChecks.NpgSql;

var builder = WebApplication.CreateBuilder(args);

var connectionString=builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
Console.WriteLine($"Connection string: {connectionString}");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseNpgsql(connectionString));

builder.Services.AddHealthChecks()
.AddNpgSql(connectionString);


var app = builder.Build();

Console.WriteLine($"Try to add");

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


app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(
                    new { status = report.Status.ToString() }
                );
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        });
        
app.UseAuthorization();
app.MapControllers();

app.Run();
