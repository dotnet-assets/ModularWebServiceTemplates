using MediatR;

namespace ModularWebService.WebServiceModule.Contracts;

public record GetTemplateByIdRequest(
        int TemplateId)
    : IRequest<TemplateDto>;