using System.Text;
using arkbo_inventory.Features.Users;
using arkbo_inventory.Features.Devices;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Infrastructure.Auth;
using arkbo_inventory.Infrastructure.Services;
using arkbo_inventory.Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(); 
builder.Services.AddHttpClient<TraccarService>(client =>
{
    client.BaseAddress = new Uri("https://gpsapi.serversync.work/api/");
});

builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddScoped<IDeviceService, CreateDeviceService>();
builder.Services.AddScoped<IUserServices, CreateUserServices>();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ARKBO Inventory API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapLogin();
app.MapUserEndpoints();
app.MapCustomer();
app.MapCustomerLogin();
app.MapDeviceEndpoints();
app.MapRoles();
app.UseSession();

// Seeding initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    db.Database.EnsureCreated();

    // Seeding roles 
    var existingRoles= await db.arkbo_roles
        .Where(r=>r.RoleDescription=="Admin"||
                  r.RoleDescription=="Distributor"||
                  r.RoleDescription=="Retailer"||
                  r.RoleDescription=="Customer")
        .Select(r=>r.RoleDescription)
        .ToListAsync();

    var rolesToAdd=new List<string> { "Admin", "Distributor", "Retailer", "Customer" }
        .Where(r => !existingRoles.Contains(r))
        .Select(r=>new Role{RoleDescription=r}).ToList();
    
    if (rolesToAdd.Any())
    {
        db.arkbo_roles.AddRange(rolesToAdd);
        await db.SaveChangesAsync();
    }
    
    // Seeding default admin 
    if (!db.arkbo_users.Any(u => u.Email == "admin@arkbo.com"))
    {
        var adminRoleId = db.arkbo_roles.Where(r => r.RoleDescription == "Admin")
            .Select(r => r.RoleId)
            .FirstOrDefault();
        db.arkbo_users.Add(new User
        {
            Email = "admin@arkbo.com",
            Password = "admin123",
            Phone = "",
            RoleId = adminRoleId
        });
        db.SaveChanges();
    }
}

app.Run(); 