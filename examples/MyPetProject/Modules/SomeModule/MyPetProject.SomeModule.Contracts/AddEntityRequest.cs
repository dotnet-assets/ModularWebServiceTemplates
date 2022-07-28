using MediatR;

namespace MyPetProject.SomeModule.Contracts;

public record AddEntityRequest(
        string SomeProperty)
    : IRequest<EntityDto>;