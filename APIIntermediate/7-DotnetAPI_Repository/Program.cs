using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
    // .AddJsonOptions(options =>
    //     {
    //         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //         options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    //     });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContextEF>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCors",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200","http://localhost:3000","http://localhost:8080")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                );
            });

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseCors("MyCors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";//string.Empty;
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
