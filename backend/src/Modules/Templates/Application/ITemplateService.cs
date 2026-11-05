namespace NotificationManagement.Templates.Application;

public interface ITemplateService
{
    Task<TemplateSummary> CreateAsync(TemplateInput input, CancellationToken cancellationToken);
    Task<TemplateSummary?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TemplateSummary>> ListAsync(CancellationToken cancellationToken);
    Task<TemplateSummary?> UpdateAsync(Guid id, TemplateInput input, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<RenderedTemplate?> RenderAsync(Guid id, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken);
}
