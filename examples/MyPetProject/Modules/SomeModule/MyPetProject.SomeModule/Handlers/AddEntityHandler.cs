using MediatR;
using MyPetProject.SomeModule.Contracts;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule.Handlers;

internal class AddEntityHandler : IRequestHandler<AddEntityRequest, EntityDto>
{
    public AddEntityHandler(SomeModuleDbContext dbContext)
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

    private readonly SomeModuleDbContext _dbContext;
}