using MediatR;
using Microsoft.EntityFrameworkCore;
using MyPetProject.SomeModule.Contracts;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule.Handlers;

internal class GetEntitiesHandler : IRequestHandler<GetEntitiesRequest, List<EntityDto>>
{
    public GetEntitiesHandler(SomeModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EntityDto>> Handle(GetEntitiesRequest request, CancellationToken cancellationToken)
    {
        List<Entity> entities = await _dbContext.Entities.ToListAsync(cancellationToken);
        List<EntityDto> dto = entities
            .Select(x => new EntityDto(x.Id, x.SomeProperty))
            .ToList();

        return dto;
    }

    private readonly SomeModuleDbContext _dbContext;
}