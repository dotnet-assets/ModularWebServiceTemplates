using MediatR;

namespace MyPetProject.SomeModule.Contracts;

public record DeleteEntityRequest(
        int EntityId)
    : IRequest;