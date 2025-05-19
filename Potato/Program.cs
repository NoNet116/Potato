using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Potato.DbContext.Models.Entity;
using Potato.DbContext;
using AutoMapper;
using Potato.Mapper;
using Potato.DbContext.Repository;
using Potato.Helpers;

var builder = WebApplication.CreateBuilder(args);

var mapperCOnfig = new MapperConfiguration((v) => { v.AddProfile(new MappingProfile()); });
IMapper mapper = mapperCOnfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Настраиваем контекст базы данных
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Scoped);

// Регистрация UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Альтернативный способ, если FriendRepository реализует IRepository<Friend>
//builder.Services.AddScoped<FriendsRepository>();

builder.Services.AddScoped<IRepository<Friend>, FriendsRepository>();

builder.Services.AddScoped<IRepository<Message>, MessageRepository>();



// Добавляем Identity с конфигурацией пароля
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 2;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

//автообновление сообщений
builder.Services.AddSignalR();

var app = builder.Build();

//автообновление сообщений
app.MapHub<ChatHub>("/chathub");



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "profile",
    pattern: "{UserName:regex(^(?!Register$|Login$|About$|Contact$).+)}",
    defaults: new { controller = "Home", action = "Profile" }
);




app.Run();
