using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record GetEntityByIdRequest(
        int EntityId)
    : IRequest<EntityDto>;