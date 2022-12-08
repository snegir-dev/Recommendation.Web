using MediatR;
using Recommendation.Application.CQs.Tag.Queries.GetAllTags;

namespace Recommendation.Application.CQs.Category.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<GetAllCategoriesVm>
{
}