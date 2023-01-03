using Blog.Domain.Models;
using MediatR;
using Blog.Application.Interfaces;
using Blog.Application.Caching;

namespace Blog.Application.Ratings.Commands.CreateRating;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, int>
{
    private readonly IBlogDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public CreateRatingCommandHandler(IBlogDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }
    public async Task<int> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = new Rating
        {
            UserId = request.UserId,
            Score = request.Score,
            ArticleId = request.ArticleId,
          
        };
        await _dbContext.Ratings.AddAsync(rating, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _cacheService.DeleteAsync($"Article {request.ArticleId}");
        return rating.Id;
    }

}
