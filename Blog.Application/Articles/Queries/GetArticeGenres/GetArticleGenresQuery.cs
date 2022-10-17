using MediatR;

namespace Blog.Application.Articles.Queries.GetArticeGenres;

public class GetArticleGenresQuery : IRequest<GenresList>
{
    public int CountGenres { get; set; }
}
