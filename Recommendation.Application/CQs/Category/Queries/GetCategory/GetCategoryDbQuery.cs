using MediatR;

namespace Recommendation.Application.CQs.Category.Queries.GetCategory;

public class GetCategoryDbQuery : IRequest<Domain.Category>
{
    public string Category { get; set; }

    public GetCategoryDbQuery(string category)
    {
        Category = category;
    }
}