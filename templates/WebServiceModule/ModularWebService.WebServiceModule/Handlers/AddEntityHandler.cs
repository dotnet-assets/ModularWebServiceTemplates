using MediatR;
using ModularWebService.WebServiceModule.Contracts;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule.Handlers;

internal class AddEntityHandler : IRequestHandler<AddEntityRequest, EntityDto>
{
    public AddEntityHandler(WebServiceModuleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EntityDto> Handle(AddEntityRequest request, CancellationToken cancellationToken)
    {
        Entity entity = new(request.SomeProperty);
        await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        EntityDto dto = new(entity.Id, entity.SomeProperty);
        return dto;
    }

    private readonly WebServiceModuleDbContext _dbContext;
}