using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Users.Queries.GetStatistics;

public class GetStatisticsQuery : IRequest<StatisticsDTO>
{
    public Role Role { get; set; }
}
