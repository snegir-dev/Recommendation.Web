using System.Collections;
using MediatR;

namespace Recommendation.Application.CQs.Tag.Queries.GetListTagContainsNames;

public class GetListTagDbContainsNamesQuery : IRequest<IEnumerable<Domain.Tag>>
{
    public string[] Tags { get; set; }

    public GetListTagDbContainsNamesQuery(string[] tags)
    {
        Tags = tags;
    }
}