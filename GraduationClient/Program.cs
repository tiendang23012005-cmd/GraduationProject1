using GraduationClient.Components;
using GraduationClient.Services;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// HTTP CLIENT
// ========================================

builder.Services.AddHttpClient<ApiService>();


// ========================================
// AUTHENTICATION STATE
// ========================================

builder.Services.AddScoped<AuthService>();


// ========================================
// BLAZOR
// ========================================

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();


// ========================================
// BUILD
// ========================================

var app = builder.Build();


// ========================================
// HTTP PIPELINE
// ========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();


// ========================================
// BLAZOR COMPONENTS
// ========================================

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();