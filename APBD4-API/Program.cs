using APBD4_API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inicjalizacja z przykładowymi danymi
DataStore.Animals.Add(new Animal { Id = 1, Name = "Java", Category = "Dog", Weight = 28.5, FurColor = "Beige" });
DataStore.Animals.Add(new Animal { Id = 2, Name = "Kiziak", Category = "Cat", Weight = 5.2, FurColor = "Black" });

DataStore.Visits.Add(new Visit { Id = 1, AnimalId = 1, Date = DateTime.Now.AddDays(-1), Description = "Annual Vaccination", Price = 100.00M });
DataStore.Visits.Add(new Visit { Id = 2, AnimalId = 2, Date = DateTime.Now, Description = "General Checkup", Price = 50.00M });

app.MapGet("/api/animals", () => DataStore.Animals);

app.MapGet("/api/animals/{id}", (int id) =>
{
    var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
    return animal != null ? Results.Ok(animal) : Results.NotFound();
});

app.MapPost("/api/animals", (Animal animal) =>
{
    DataStore.Animals.Add(animal);
    return Results.Created($"/api/animals/{animal.Id}", animal);
});

app.MapPut("/api/animals/{id}", (int id, Animal updatedAnimal) =>
{
    var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
    if (animal == null)
    {
        return Results.NotFound();
    }
    animal.Name = updatedAnimal.Name;
    animal.Category = updatedAnimal.Category;
    animal.Weight = updatedAnimal.Weight;
    animal.FurColor = updatedAnimal.FurColor;
    return Results.NoContent();
});

app.MapDelete("/api/animals/{id}", (int id) =>
{
    var animal = DataStore.Animals.FirstOrDefault(a => a.Id == id);
    if (animal == null)
    {
        return Results.NotFound();
    }
    DataStore.Animals.Remove(animal);
    return Results.NoContent();
});

app.MapGet("/api/visits", () => DataStore.Visits);

app.MapGet("/api/visits/{animalId}", (int animalId) =>
{
    var visits = DataStore.Visits.Where(v => v.AnimalId == animalId).ToList();
    return visits.Any() ? Results.Ok(visits) : Results.NotFound();
});

app.MapPost("/api/visits", (Visit visit) =>
{
    DataStore.Visits.Add(visit);
    return Results.Created($"/api/visits/{visit.Id}", visit);
});

app.Run();
