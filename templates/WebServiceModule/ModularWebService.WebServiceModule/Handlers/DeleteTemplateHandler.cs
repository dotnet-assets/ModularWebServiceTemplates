using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Modularity;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class DeleteTemplateHandler : IRequestHandler<DeleteTemplateRequest>
{
    public DeleteTemplateHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTemplateRequest request, CancellationToken cancellationToken)
    {
        Template? model = await _dbContext.Templates
            .FirstOrDefaultAsync(x => x.Id == request.TemplateId, cancellationToken);
        if (model is null)
        {
            throw new AppException("Template not found");
        }

        _dbContext.Templates.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}