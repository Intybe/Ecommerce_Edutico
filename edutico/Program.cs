var builder = WebApplication.CreateBuilder(args);

// Adiciona o servi�o de sess�o
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); // Adiciona suporte ao HttpContext

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ativa o uso da sess�o
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
