using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Modularity;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class GetTemplateByIdHandler : IRequestHandler<GetTemplateByIdRequest, TemplateDto>
{
    public GetTemplateByIdHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TemplateDto> Handle(GetTemplateByIdRequest request, CancellationToken cancellationToken)
    {
        Template? model = await _dbContext.Templates
            .FirstOrDefaultAsync(x => x.Id == request.TemplateId, cancellationToken);
        if (model is null)
        {
            throw new AppException("Template not found");
        }

        TemplateDto dto = new(model.Id, model.SomeProperty);
        return dto;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}