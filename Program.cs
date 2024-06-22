using Assignment_Campus_Placement.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var cosmosDbSection = configuration.GetSection("CosmosDb");
    return new CosmosClient(cosmosDbSection["Account"], cosmosDbSection["Key"]);
});

builder.Services.AddScoped<IQuestionService, QuestionService>(serviceProvider =>
{
    var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var cosmosDbSection = configuration.GetSection("CosmosDb");
    return new QuestionService(cosmosClient, cosmosDbSection["DatabaseName"], "Questions");
});

builder.Services.AddScoped<ICandidateApplicationService, CandidateApplicationService>(serviceProvider =>
{
    var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var cosmosDbSection = configuration.GetSection("CosmosDb");
    return new CandidateApplicationService(cosmosClient, cosmosDbSection["DatabaseName"], "CandidateApplications");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Candidate Application API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
