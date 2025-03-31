using Microsoft.EntityFrameworkCore;
using MedicoPrenotazioni.Data;
using MedicoPrenotazioni.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurazione della connessione al database SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Registrazione dei servizi dell'applicazione
builder.Services.AddScoped<IOperatoreService, OperatoreService>();
builder.Services.AddScoped<IServizioService, ServizioService>();
builder.Services.AddScoped<ISlotDisponibilitaService, SlotDisponibilitaService>();
builder.Services.AddScoped<IPrenotazioneService, PrenotazioneService>();
builder.Services.AddScoped<INonDisponibileService, NonDisponibileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Inizializzazione del database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Si è verificato un errore durante la migrazione del database.");
    }
}

app.Run();
