using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Application.Ratings.Queries.GetRatingByArticle;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;

public class GetRatingListByUserQueryHandler : IRequestHandler<GetRatingListByUserQuery, RatingListVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetRatingListByUserQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<RatingListVm> Handle(GetRatingListByUserQuery request, CancellationToken cancellationToken)
    {

        var ratingQuery = await _dbContext.Ratings
            .Where(rating => rating.UserId == request.UserId)
            .ProjectTo<RatingLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (request.UserId == Guid.Empty)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        return new RatingListVm { Ratings = ratingQuery };
    }

}
