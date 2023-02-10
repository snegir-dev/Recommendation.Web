using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Recommendation.Application.CQs.AuthenticationScheme.Queries.GetSpecifiedAuthenticationScheme;

public class GetSpecifiedAuthenticationSchemeQuery 
    : IRequest<AuthenticationProperties>
{
    public string Provider { get; set; }

    public GetSpecifiedAuthenticationSchemeQuery(string provider)
    {
        Provider = provider;
    }
}