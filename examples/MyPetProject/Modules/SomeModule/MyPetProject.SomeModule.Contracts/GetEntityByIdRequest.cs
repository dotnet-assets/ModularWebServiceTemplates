using MediatR;

namespace MyPetProject.SomeModule.Contracts;

public record GetEntityByIdRequest(
        int EntityId)
    : IRequest<EntityDto>;