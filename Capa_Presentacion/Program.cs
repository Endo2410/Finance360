var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ? Habilitar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ? Usar sesión
app.UseSession();

app.UseAuthorization();

// Ruta principal del sistema (login)
app.MapControllerRoute(
    name: "login",
    pattern: "",
    defaults: new { controller = "Acceso", action = "Index" }
);

// Ruta normal cuando escriben controlador/acción
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}"
);

// Si escriben solo el controlador
app.MapControllerRoute(
    name: "soloControlador",
    pattern: "{controller}",
    defaults: new { controller = "Home", action = "LostInSpace" }
);

// Cualquier otra ruta inválida
app.MapControllerRoute(
    name: "catchAll",
    pattern: "{*url}",
    defaults: new { controller = "Home", action = "LostInSpace" }
);

app.Run();
