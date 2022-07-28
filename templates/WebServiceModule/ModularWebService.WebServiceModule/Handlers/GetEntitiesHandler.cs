using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class GetEntitiesHandler : IRequestHandler<GetEntitiesRequest, List<EntityDto>>
{
    public GetEntitiesHandler(WebServiceModuleDbContext dbContext)
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

    private readonly WebServiceModuleDbContext _dbContext;
}