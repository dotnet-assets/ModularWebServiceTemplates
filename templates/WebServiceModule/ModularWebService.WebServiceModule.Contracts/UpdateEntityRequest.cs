using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record UpdateEntityRequest(
        int Id,
        string SomeProperty)
    : IRequest<EntityDto>;