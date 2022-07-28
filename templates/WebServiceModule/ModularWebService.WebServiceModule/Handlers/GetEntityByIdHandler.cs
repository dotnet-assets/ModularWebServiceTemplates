using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Modularity;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class GetEntityByIdHandler : IRequestHandler<GetEntityByIdRequest, EntityDto>
{
    public GetEntityByIdHandler(WebServiceModuleDbContext dbContext)
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

    private readonly WebServiceModuleDbContext _dbContext;
}