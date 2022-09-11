using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQueryHandler : IRequestHandler<GetCommentsByArticleQuery, CommentListVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetCommentsByArticleQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<CommentListVm> Handle(GetCommentsByArticleQuery request, CancellationToken cancellationToken)
    {
        var commentQuery = await _dbContext.Comments
            .Where(comment => comment.ArticleId == request.ArticleId)
            .ProjectTo<CommentLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (request.ArticleId == Guid.Empty)
        {
            throw new NotFoundException(nameof(Article), request.ArticleId);
        }

        return new CommentListVm { Comments = commentQuery };
       
    }
}
