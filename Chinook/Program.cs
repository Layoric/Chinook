using System.Globalization;
using Chinook;
using Chinook.ServiceInterface;
using Microsoft.AspNetCore.Localization;
using ServiceStack;

ServiceStack.Licensing.RegisterLicense("OSS GPL-3.0 2024 https://github.com/NetCoreApps/Chinook I9j/48yaOXHt2eVVVZczPzbcGfC1k5njkND7V84Q/PLAOYHFx8KbmXHCluErNL+OeBaS+RnAZv3DEM6xKoG9gOB9ucRPCdpIoyxqhEaf40LonG+6GI7O+30t3WvVQJGI65Q64q+uVsRb025bysaEHfOf7Lf5YcKmwZvkao4nlfU=");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register all services
builder.Services.AddServiceStack(typeof(MyServices).Assembly, c => {
    c.AddSwagger(o => {
        //o.AddJwtBearer();
        o.AddBasicAuth();
    });
});

var supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-GB"),
    new CultureInfo("en-US"),
    new CultureInfo("hu-HU")
};

var requestLocalizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.DefaultRequestCulture = new RequestCulture(requestLocalizationOptions.DefaultRequestCulture.Culture.Name);
    options.SupportedCultures = requestLocalizationOptions.SupportedCultures;
    options.SupportedUICultures = requestLocalizationOptions.SupportedCultures;
});

var app = builder.Build();

app.UseRequestLocalization(requestLocalizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseServiceStack(new AppHost(), options =>
{
    options.MapEndpoints();
});

app.Run();