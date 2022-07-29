using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record AddTemplateRequest(
        string SomeProperty)
    : IRequest<TemplateDto>;