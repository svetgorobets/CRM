using Microsoft.EntityFrameworkCore;
using NotificationManagement.Customers.Application;
using NotificationManagement.Customers.Infrastructure;
using NotificationManagement.Integrations.Application;
using NotificationManagement.Integrations.Infrastructure;
using NotificationManagement.Templates.Application;
using NotificationManagement.Templates.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddCustomersModule(builder.Configuration);
builder.Services.AddTemplatesModule(builder.Configuration);
builder.Services.AddIntegrationsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<CustomersDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<TemplatesDbContext>().Database.MigrateAsync();
}

app.MapGet("/", () => Results.Ok(new { service = "notification-management-api" }));
app.MapHealthChecks("/health");

var customers = app.MapGroup("/api/customers");
customers.MapGet("/", async (string? search, ICustomerService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.ListAsync(search, cancellationToken)));
customers.MapGet("/{id:guid}", async (Guid id, ICustomerService service, CancellationToken cancellationToken) =>
{
    var customer = await service.GetAsync(id, cancellationToken);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
});
customers.MapPost("/", async (CustomerInput input, ICustomerService service, CancellationToken cancellationToken) =>
{
    var errors = CustomerInputValidator.Validate(input);
    if (errors.Count > 0) return Results.ValidationProblem(errors);

    try
    {
        var customer = await service.CreateAsync(input, cancellationToken);
        return Results.Created($"/api/customers/{customer.Customer.Id}", customer);
    }
    catch (CustomerValidationException exception)
    {
        return Results.ValidationProblem(exception.Errors);
    }
});
customers.MapPut("/{id:guid}", async (Guid id, CustomerUpdateInput input, ICustomerService service, CancellationToken cancellationToken) =>
{
    var errors = CustomerInputValidator.Validate(input);
    if (errors.Count > 0) return Results.ValidationProblem(errors);

    try
    {
        var customer = await service.UpdateAsync(id, input, cancellationToken);
        return customer is null ? Results.NotFound() : Results.Ok(customer);
    }
    catch (CustomerValidationException exception)
    {
        return Results.ValidationProblem(exception.Errors);
    }
});
customers.MapPut("/{id:guid}/preferences", async (Guid id, CustomerPreferenceInput input, ICustomerService service, CancellationToken cancellationToken) =>
{
    var customer = await service.SetPreferenceAsync(id, input, cancellationToken);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
});

var templates = app.MapGroup("/api/templates");
templates.MapGet("/", async (ITemplateService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.ListAsync(cancellationToken)));
templates.MapGet("/{id:guid}", async (Guid id, ITemplateService service, CancellationToken cancellationToken) =>
{
    var template = await service.GetAsync(id, cancellationToken);
    return template is null ? Results.NotFound() : Results.Ok(template);
});
templates.MapPost("/", async (TemplateInput input, ITemplateService service, CancellationToken cancellationToken) =>
{
    var template = await service.CreateAsync(input, cancellationToken);
    return Results.Created($"/api/templates/{template.Id}", template);
});
templates.MapPut("/{id:guid}", async (Guid id, TemplateInput input, ITemplateService service, CancellationToken cancellationToken) =>
{
    var template = await service.UpdateAsync(id, input, cancellationToken);
    return template is null ? Results.NotFound() : Results.Ok(template);
});
templates.MapDelete("/{id:guid}", async (Guid id, ITemplateService service, CancellationToken cancellationToken) =>
    await service.DeleteAsync(id, cancellationToken) ? Results.NoContent() : Results.NotFound());
templates.MapPost("/{id:guid}/render", async (Guid id, RenderTemplateRequest request, ITemplateService service, CancellationToken cancellationToken) =>
{
    var rendered = await service.RenderAsync(id, request.Values, cancellationToken);
    return rendered is null ? Results.NotFound() : Results.Ok(rendered);
});

var integrations = app.MapGroup("/api/integrations");
integrations.MapGet("/", (IIntegrationConfigurationService service) => Results.Ok(service.GetActiveConfigurations()));

app.Run();

public sealed record RenderTemplateRequest(Dictionary<string, string> Values);
