using MediatR;
using Microsoft.EntityFrameworkCore;
using MyPetProject.Modularity;
using MyPetProject.SomeModule.Contracts;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule.Handlers;

internal class UpdateEntityHandler : IRequestHandler<UpdateEntityRequest, EntityDto>
{
    public UpdateEntityHandler(SomeModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EntityDto> Handle(UpdateEntityRequest request, CancellationToken cancellationToken)
    {
        Entity? entity = await _dbContext.Entities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw new AppException("Entity not found");
        }

        entity.Update(request.SomeProperty);

        EntityDto dto = new(entity.Id, entity.SomeProperty);
        return dto;
    }

    private readonly SomeModuleDbContext _dbContext;
}