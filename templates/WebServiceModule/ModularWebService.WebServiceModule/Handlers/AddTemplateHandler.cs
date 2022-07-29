using MediatR;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class AddTemplateHandler : IRequestHandler<AddTemplateRequest, TemplateDto>
{
    public AddTemplateHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TemplateDto> Handle(AddTemplateRequest request, CancellationToken cancellationToken)
    {
        Template model = new(request.SomeProperty);
        await _dbContext.Templates.AddAsync(model, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TemplateDto dto = new(model.Id, model.SomeProperty);
        return dto;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}