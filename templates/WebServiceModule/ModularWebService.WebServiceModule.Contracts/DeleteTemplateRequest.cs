using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record DeleteTemplateRequest(
        int TemplateId)
    : IRequest;