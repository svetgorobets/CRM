using System.Text.RegularExpressions;
using NotificationManagement.BuildingBlocks.Domain;
using NotificationManagement.Templates.Domain;

namespace NotificationManagement.Templates.Application;

public sealed record TemplateInput(string Name, NotificationChannel Channel, string? Subject, string Content, bool IsActive = true);
public sealed record TemplateSummary(Guid Id, string Name, NotificationChannel Channel, string? Subject, string Content, bool IsActive, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
public sealed record RenderedTemplate(string? Subject, string Content);

public interface ITemplateRepository
{
    Task<NotificationTemplate?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<NotificationTemplate>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken);
    void Remove(NotificationTemplate template);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ITemplateService
{
    Task<TemplateSummary> CreateAsync(TemplateInput input, CancellationToken cancellationToken);
    Task<TemplateSummary?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken);
    Task<TemplateSummary?> UpdateAsync(Guid id, TemplateInput input, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<RenderedTemplate?> RenderAsync(Guid id, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken);
}

public sealed class TemplateService(ITemplateRepository repository, TimeProvider timeProvider) : ITemplateService
{
    private static readonly Regex Placeholder = new("{{\\s*(?<key>[a-zA-Z0-9_.-]+)\\s*}}", RegexOptions.Compiled);

    public async Task<TemplateSummary> CreateAsync(TemplateInput input, CancellationToken cancellationToken)
    {
        var template = NotificationTemplate.Create(input.Name, input.Channel, input.Subject, input.Content, timeProvider.GetUtcNow());
        if (!input.IsActive) template.Update(input.Name, input.Subject, input.Content, false, timeProvider.GetUtcNow());
        await repository.AddAsync(template, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return ToSummary(template);
    }

    public async Task<TemplateSummary?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var template = await repository.FindAsync(id, cancellationToken);
        return template is null ? null : ToSummary(template);
    }

    public async Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken) =>
        (await repository.ListAsync(cancellationToken)).Select(ToSummary).ToArray();

    public async Task<TemplateSummary?> UpdateAsync(Guid id, TemplateInput input, CancellationToken cancellationToken)
    {
        var template = await repository.FindAsync(id, cancellationToken);
        if (template is null) return null;

        template.Update(input.Name, input.Subject, input.Content, input.IsActive, timeProvider.GetUtcNow());
        await repository.SaveChangesAsync(cancellationToken);
        return ToSummary(template);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var template = await repository.FindAsync(id, cancellationToken);
        if (template is null) return false;

        repository.Remove(template);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<RenderedTemplate?> RenderAsync(Guid id, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken)
    {
        var template = await repository.FindAsync(id, cancellationToken);
        if (template is null) return null;

        return new RenderedTemplate(Render(template.Subject, values), Render(template.Content, values)!);
    }

    private static string? Render(string? template, IReadOnlyDictionary<string, string> values) =>
        template is null ? null : Placeholder.Replace(template, match =>
            values.TryGetValue(match.Groups["key"].Value, out var value) ? value : match.Value);

    private static TemplateSummary ToSummary(NotificationTemplate template) =>
        new(template.Id, template.Name, template.Channel, template.Subject, template.Content, template.IsActive, template.CreatedAt, template.UpdatedAt);
}
