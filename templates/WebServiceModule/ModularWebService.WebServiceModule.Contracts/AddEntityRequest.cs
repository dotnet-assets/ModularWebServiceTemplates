using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record AddEntityRequest(
        string SomeProperty)
    : IRequest<EntityDto>;