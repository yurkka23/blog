using System;
using Blog.Domain.Enums;
using Blog.Domain.Models;
using MediatR;
using Blog.Domain;
using Blog.Application.Interfaces;

namespace Blog.Application.Ratings.Commands.CreateRating;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, int>
{
    private readonly IBlogDbContext _dbContext;
    public CreateRatingCommandHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
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

        return rating.Id;
    }

}
