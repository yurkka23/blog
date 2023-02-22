using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;

namespace Blog.Application.Users.Queries.GetProfileUser;

public class GetProfileUserQueryHandler : IRequestHandler<GetProfileUserQuery, ProfileUser>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetProfileUserQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ProfileUser> Handle(GetProfileUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

        if (userQuery == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        var result = _mapper.Map<ProfileUser>(userQuery);
        result.CountArticles = await _dbContext.Articles.CountAsync(art => art.CreatedBy == request.Id && art.State == Domain.Enums.State.Approved, cancellationToken);

        result.Followers = _dbContext.UserSubscriptions
            .Count(user => user.UserToSubscribeId == request.Id);

        result.Following = _dbContext.UserSubscriptions
            .Count(user => user.UserId == request.Id);

        result.IsCurrentUserSubscribed = _dbContext.UserSubscriptions
            .Any(user => user.UserId == request.CurrentUserId && user.UserToSubscribeId == request.Id);

        return result;
    }
}
