using System.Collections.Generic;
using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record GetEntitiesRequest : IRequest<List<EntityDto>>;