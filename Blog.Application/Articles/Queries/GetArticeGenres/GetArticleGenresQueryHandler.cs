using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Blog.Domain.Models;

namespace Blog.Application.Articles.Queries.GetArticeGenres;

public class GetArticleGenresQueryHandler : IRequestHandler<GetArticleGenresQuery, GenresList>
{
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Article> _entitiesCollection;

    public GetArticleGenresQueryHandler(IOptions<MongoEntitiesDBSettings> entitiesStoreDatabaseSettings, IMapper mapper)
    {
        _mapper = mapper;
        var mongoClient = new MongoClient(
           entitiesStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            entitiesStoreDatabaseSettings.Value.DatabaseName);

        _entitiesCollection = mongoDatabase.GetCollection<Article>(
            entitiesStoreDatabaseSettings.Value.CollectionName);
    }
    public async Task<GenresList> Handle(GetArticleGenresQuery request, CancellationToken cancellationToken)
    {
       
        var genreList =(await _entitiesCollection.FindAsync(Builders<Article>.Filter.Eq("_t", "Article")))
            .ToEnumerable()
            .Select(genre => genre.Genre)
            .Where(genre => genre != null)
            .Distinct()
            .OrderBy(genre => genre)
            .Take(request.CountGenres)
            .ToList();

        return new GenresList { Genres = genreList };
    }
}

