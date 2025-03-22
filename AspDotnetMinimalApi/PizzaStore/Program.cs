using Microsoft.OpenApi.Models;
// using PizzaStore.DB;
using PizzaStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";
var connectionString = builder.Configuration.GetConnectionString("Pizzas");

// builder.Services.AddDbContext<PizzaDb>(options=> options.UseInMemoryDatabase("items"));
// builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddDbContext<PizzaDb>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
});
    
var app = builder.Build();
    
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
   });
}
    
// app.MapGet("/", () => "Hello World!");
// app.MapGet("pizzas/id", (int id)=> PizzaDbStatic.GetPizza(id));
// app.MapGet("pizzas", ()=> PizzaDbStatic.GetPizzas());
// app.MapPost("pizzas", (Pizza p)=> PizzaDbStatic.CreatePizza(p));
// app.MapPut("pizzas", (Pizza p)=> PizzaDbStatic.UpdatePizza(p));
// app.MapDelete("pizzas/{id}", (int id)=> PizzaDbStatic.RemovePizaa(id));
 
app.MapGet("pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
app.MapGet("pizzas", async (PizzaDb db)=> await db.Pizzas.ToListAsync());
 app.MapPost("pizzas", async (PizzaDb db, Pizza p)=> {
    await db.Pizzas.AddAsync(p);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{p.Id}", p);
 });
app.MapPut("pizza/{id}", async (PizzaDb db, Pizza p, int id)=> {
    if(id!=p.Id) return Results.BadRequest();
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null) return Results.NotFound();
    pizza.Name=p.Name;
    pizza.Description=p.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("pizza/{id}", async (PizzaDb db, int id)=> {
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null) return Results.NotFound();
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();