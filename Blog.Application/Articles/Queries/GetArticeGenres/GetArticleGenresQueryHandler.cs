using AutoMapper;
using Blog.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Articles.Queries.GetArticeGenres;

public class GetArticleGenresQueryHandler : IRequestHandler<GetArticleGenresQuery, GenresList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetArticleGenresQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<GenresList> Handle(GetArticleGenresQuery request, CancellationToken cancellationToken)
    {
        var genreList = await _dbContext.Articles
            .Select(genre => genre.Genre)
            .Distinct()
            .OrderBy(genre => genre)
            .Take(request.CountGenres)
            .ToListAsync(cancellationToken);

        return new GenresList { Genres = genreList };
    }
}

