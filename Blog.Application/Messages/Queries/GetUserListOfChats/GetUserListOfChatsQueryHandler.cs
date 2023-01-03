using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class GetUserListOfChatsQueryHandler : IRequestHandler<GetUserListOfChatsQuery, ListOfChats>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetUserListOfChatsQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ListOfChats> Handle(GetUserListOfChatsQuery request, CancellationToken cancellationToken)
    {
        var listUserId = await _dbContext.Messages
             .Where(m =>
            m.RecipienId == request.UserId && m.RecipientDeleted == false ||
            m.SenderId == request.UserId && m.SenderDeleted == false 
            ).OrderByDescending(m => m.MessageSent)
            .Select(c => c.RecipienId == request.UserId ? c.SenderId : c.RecipienId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var chats = await _dbContext.Users
            .Where(u => listUserId.Contains(u.Id))
            .Select(u => new ChatDTO
            {
                RecipientUserId = u.Id,
                RecipientAvatarUrl = u.ImageUserUrl,
                RecipientUsername = u.UserName
            })
            .ToListAsync(cancellationToken);

        return new ListOfChats { Chats = chats };
    }

}
