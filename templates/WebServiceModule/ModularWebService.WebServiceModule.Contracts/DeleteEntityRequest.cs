using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record DeleteEntityRequest(
        int EntityId)
    : IRequest;