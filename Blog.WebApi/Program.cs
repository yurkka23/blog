using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

//add services to the cointainer

builder.Services.Configure<CacheStoreDatabaseSettings>(
    builder.Configuration.GetSection("CachingStoreDatabase"));

builder.Services.Configure<MongoUserDBSettings>(
  builder.Configuration.GetSection("MongoUserDBSettings"));

builder.Services.Configure<MongoEntitiesDBSettings>(
  builder.Configuration.GetSection("MongoEntitiesDBSettings"));

builder.Services.Configure<MongoConnectionsDBSettings>(
  builder.Configuration.GetSection("MongoConnectionsDBSettings"));

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddSingleton<PresenceTracker>();
builder.Services.AddApplication(builder.Configuration);
//builder.Services.AddPersistance(builder.Configuration);

//mongo settings

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoUserDBSettings)).Get<MongoUserDBSettings>();

builder.Services.AddIdentity<User, ApplicationRole>()
    .AddMongoDbStores<User, ApplicationRole, Guid>
    (
        mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName
    );

//
builder.Services.AddControllers();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(UserInfo).Assembly));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200");
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//builder.Services.AddIdentity<User, ApplicationRole>()
//    .AddEntityFrameworkStores<BlogDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer",options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
});


builder.Services.AddHttpClient();


//build pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = String.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog v1");
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

//clear table connection 
 IMongoCollection<Connection> _connectionsCollection;

var mongoClient2 = new MongoClient(
           app.Configuration.GetSection("MongoConnectionsDBSettings:ConnectionString").Value);

var mongoDatabase2 = mongoClient2.GetDatabase(
    app.Configuration.GetSection("MongoConnectionsDBSettings:DatabaseName").Value);

_connectionsCollection = mongoDatabase2.GetCollection<Connection>(
     app.Configuration.GetSection("MongoConnectionsDBSettings:CollectionName").Value);

_connectionsCollection.DeleteMany(x => true);
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//var context = services.GetRequiredService<BlogDbContext>();
//await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
//await context.Database.ExecuteSqlRawAsync("Delete from [Connections]");
//context.Connections.RemoveRange(context.Connections);
//await context.SaveChangesAsync();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();



app.Run();