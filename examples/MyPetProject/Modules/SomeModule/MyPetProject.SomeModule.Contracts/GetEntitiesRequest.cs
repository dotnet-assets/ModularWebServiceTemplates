using System.Collections.Generic;
using MediatR;

namespace MyPetProject.SomeModule.Contracts;

public record GetEntitiesRequest : IRequest<List<EntityDto>>;