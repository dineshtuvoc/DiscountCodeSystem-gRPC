using DiscountCode_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC services
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<DiscountServiceImpl>();
app.MapGet("/", () => "Discount gRPC Service Started...");

app.Run();
