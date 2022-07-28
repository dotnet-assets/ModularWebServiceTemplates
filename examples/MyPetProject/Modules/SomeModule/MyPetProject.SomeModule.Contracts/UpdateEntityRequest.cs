using MediatR;

namespace MyPetProject.SomeModule.Contracts;

public record UpdateEntityRequest(
        int Id,
        string SomeProperty)
    : IRequest<EntityDto>;