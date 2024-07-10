#pragma warning disable SA1200

using Services.WebApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ToDoListWebApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5130");
});
builder.Services.AddHttpClient<AccountWebApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5130");
});
builder.Services.AddHttpClient<TasksWebApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5130");
});
builder.Services.AddHttpClient<TagsWebApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5130");
});
builder.Services.AddHttpClient<CommentsWebApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5130");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TodoList}/{action=Index}/{id?}");

app.Run();
