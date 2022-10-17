using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Application.Interfaces;
using Blog.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Ratings.Queries.GetRatingListByUser;

public class GetRatingListByUserQueryHandler : IRequestHandler<GetRatingListByUserQuery, RatingList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetRatingListByUserQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<RatingList> Handle(GetRatingListByUserQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        var ratingQuery = await _dbContext.Ratings
            .Where(rating => rating.UserId == request.UserId)
            .OrderByDescending(rating => rating.Id)
            .ToListAsync(cancellationToken);

        var ratingList = new List<RatingLookupDto>();

        if (ratingQuery.Count > 0)
        {
            var index = 0;
            foreach (var rating in ratingQuery)
            {
                var getArticleInfo = await _dbContext.Articles
                .Where(article => article.Id == ratingQuery[index].ArticleId)
                .ToListAsync(cancellationToken);

                var temp = _mapper.Map<RatingLookupDto>(rating);
                temp.ArticleTitle = getArticleInfo[0].Title;
                temp.ArticleImage = getArticleInfo[0].ArticleImageUrl;
                temp.ArticleId = getArticleInfo[0].Id;
                ratingList.Add(temp);
                index++;
            }
        }



        return new RatingList { Ratings = ratingList };
    }

}
