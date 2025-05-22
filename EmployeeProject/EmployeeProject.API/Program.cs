using EmployeeProject.Services.EmployeeServices;
using EmployeeProject.Services.HelperService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Allow any origin
              .AllowAnyMethod()    // Allow any HTTP method (GET, POST, PUT, DELETE)
              .AllowAnyHeader();   // Allow any header
    });
});

// Register services
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeService, EmpService>();
builder.Services.AddScoped<IAdoHelper, AdoHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS globally
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
