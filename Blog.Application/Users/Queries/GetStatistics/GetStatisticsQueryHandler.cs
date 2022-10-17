
using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Users.Queries.GetStatistics;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDTO>
{
    private readonly IBlogDbContext _dbContext;
    public GetStatisticsQueryHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<StatisticsDTO> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        if(request.Role != Role.Admin)
        {
            throw new NotRightsException(request.Role);
        }
        var statistics = new StatisticsDTO
        {
            CountAdmins = await _dbContext.Users.CountAsync(user => user.Role == Role.Admin, cancellationToken),
            CountUsers = await _dbContext.Users.CountAsync(user => user.Role == Role.User, cancellationToken),
            CountComments  = await _dbContext.Comments.CountAsync(cancellationToken),
            CountRating = await _dbContext.Ratings.CountAsync(cancellationToken),
            ApprovedArticles = await _dbContext.Articles.CountAsync(art => art.State == State.Approved, cancellationToken),
            DeclinedArticles = await _dbContext.Articles.CountAsync(art => art.State == State.Declined, cancellationToken),
            WaitingArticles = await _dbContext.Articles.CountAsync(art => art.State == State.Waiting, cancellationToken),
        };

        return statistics;
    }
}
