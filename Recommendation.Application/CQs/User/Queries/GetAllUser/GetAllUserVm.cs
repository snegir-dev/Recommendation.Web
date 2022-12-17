namespace Recommendation.Application.CQs.User.Queries.GetAllUser;

public class GetAllUserVm
{
    public IEnumerable<GetAllUserDto> Users { get; set; }

    public GetAllUserVm(IEnumerable<GetAllUserDto> users)
    {
        Users = users;
    }
}