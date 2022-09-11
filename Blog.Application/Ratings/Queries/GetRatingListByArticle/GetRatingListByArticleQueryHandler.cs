using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Application.Ratings.Queries.GetRatingByArticle;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Ratings.Queries.GetRatingListByArticle;

public class GetRatingListByArticleQueryHandler : IRequestHandler<GetRatingListByArticleQuery, RatingListVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetRatingListByArticleQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<RatingListVm> Handle(GetRatingListByArticleQuery request, CancellationToken cancellationToken)
    {

        var ratingQuery = await _dbContext.Ratings
            .Where(rating => rating.ArticleId == request.ArticleId)
            .ProjectTo<RatingLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (request.ArticleId == Guid.Empty)
        {
            throw new NotFoundException(nameof(Article), request.ArticleId);
        }

        return new RatingListVm { Ratings = ratingQuery };
    }
}
