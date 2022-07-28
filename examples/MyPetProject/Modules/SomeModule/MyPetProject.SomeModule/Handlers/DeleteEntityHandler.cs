using MediatR;
using Microsoft.EntityFrameworkCore;
using MyPetProject.Modularity;
using MyPetProject.SomeModule.Contracts;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule.Handlers;

internal class DeleteEntityHandler : IRequestHandler<DeleteEntityRequest>
{
    public DeleteEntityHandler(SomeModuleDbContext dbContext)
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

    private readonly SomeModuleDbContext _dbContext;
}