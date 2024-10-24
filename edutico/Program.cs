using edutico.Models;
using edutico.Libraries.Login;
using edutico.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de sessão
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); // Adiciona suporte ao HttpContext

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILoginRepositorio, LoginRepositorio>();
builder.Services.AddScoped<edutico.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginSessao>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
builder.Services.AddScoped<ICartaoRepositorio, CartaoRepositorio>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ativa o uso da sessão
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

app.UseRouting();



