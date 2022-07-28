using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Modularity;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class DeleteEntityHandler : IRequestHandler<DeleteEntityRequest>
{
    public DeleteEntityHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteEntityRequest request, CancellationToken cancellationToken)
    {
        Entity? entity = await _dbContext.Entities
            .FirstOrDefaultAsync(x => x.Id == request.EntityId, cancellationToken);
        if (entity is null)
        {
            throw new AppException("Entity not found");
        }

        _dbContext.Entities.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}