using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
  .AddGraphQLServer()
  .AddQueryType<Query>()
  .AddMutationType<Mutation>()
  .AddSubscriptionType<Subscription>()
  .AddType<PlatformType>()
  .AddType<CommandType>()
  .AddFiltering()
  .AddSorting()
  .AddInMemorySubscriptions();

builder.Services.AddCors(options => options.AddPolicy("AllowWebApp",
    builder => builder.AllowAnyOrigin()
  .AllowAnyHeader()
  .AllowAnyMethod()));

var app = builder.Build();

IConfiguration configuration = app.Configuration;
IWebHostEnvironment environment = app.Environment;

app.UseWebSockets();

app.MapGraphQL("/graphql");

app.Run();

//https://youtu.be/HuN94qNwQmM