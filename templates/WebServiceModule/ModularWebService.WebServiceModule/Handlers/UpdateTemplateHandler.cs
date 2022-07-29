using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Modularity;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class UpdateTemplateHandler : IRequestHandler<UpdateTemplateRequest, TemplateDto>
{
    public UpdateTemplateHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TemplateDto> Handle(UpdateTemplateRequest request, CancellationToken cancellationToken)
    {
        Template? model = await _dbContext.Templates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (model is null)
        {
            throw new AppException("Template not found");
        }

        model.Update(request.SomeProperty);

        TemplateDto dto = new(model.Id, model.SomeProperty);
        return dto;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}