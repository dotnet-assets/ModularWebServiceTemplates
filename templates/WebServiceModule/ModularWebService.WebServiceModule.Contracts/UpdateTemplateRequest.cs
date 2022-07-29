using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record UpdateTemplateRequest(
        int Id,
        string SomeProperty)
    : IRequest<TemplateDto>;