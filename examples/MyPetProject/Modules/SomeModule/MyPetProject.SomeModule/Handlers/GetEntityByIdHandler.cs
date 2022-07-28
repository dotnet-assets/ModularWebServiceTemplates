using MediatR;
using Microsoft.EntityFrameworkCore;
using MyPetProject.Modularity;
using MyPetProject.SomeModule.Contracts;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule.Handlers;

internal class GetEntityByIdHandler : IRequestHandler<GetEntityByIdRequest, EntityDto>
{
    public GetEntityByIdHandler(SomeModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EntityDto> Handle(GetEntityByIdRequest request, CancellationToken cancellationToken)
    {
        Entity? entity = await _dbContext.Entities
            .FirstOrDefaultAsync(x => x.Id == request.EntityId, cancellationToken);
        if (entity is null)
        {
            throw new AppException("Entity not found");
        }

        EntityDto dto = new(entity.Id, entity.SomeProperty);
        return dto;
    }

    private readonly SomeModuleDbContext _dbContext;
}