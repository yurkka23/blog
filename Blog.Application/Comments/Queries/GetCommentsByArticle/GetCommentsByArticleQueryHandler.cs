using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;

namespace Blog.Application.Comments.Queries.GetCommentsByArticle;

public class GetCommentsByArticleQueryHandler : IRequestHandler<GetCommentsByArticleQuery, CommentList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetCommentsByArticleQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<CommentList> Handle(GetCommentsByArticleQuery request, CancellationToken cancellationToken)
    {
        if (request.ArticleId == Guid.Empty)
        {
            throw new NotFoundException(nameof(Article), request.ArticleId);
        }
        var commentQuery = await _dbContext.Comments
            .Where(comment => comment.ArticleId == request.ArticleId)
            .OrderByDescending(comment=> comment.Id)
            .ToListAsync(cancellationToken);

        var commetnsList = new List<CommentLookupDto>();

        if (commentQuery.Count > 0)
        {
            var index = 0;
            foreach (var comment in commentQuery)
            {
                var getAuthorName = await _dbContext.Users
                .Where(user => user.Id == commentQuery[index].UserId)
                .ToListAsync(cancellationToken);

                var temp = _mapper.Map<CommentLookupDto>(comment);
                temp.AuthorImgUrl = getAuthorName[0].ImageUserUrl;
                temp.AuthorUserName = getAuthorName[0].UserName;
                commetnsList.Add(temp);
                index++;
            }



        }


        return new CommentList { Comments = commetnsList };
    }
}
