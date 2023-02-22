using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class GetMessagesFromGroupQueryHandler : IRequestHandler<GetMessagesFromGroupQuery, MessagesList>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetMessagesFromGroupQueryHandler(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<MessagesList> Handle(GetMessagesFromGroupQuery request, CancellationToken cancellationToken)
    {
        var messages = _dbContext.Messages
             .Where(m =>
            m.RecipienId == request.CurrentUserId && m.RecipientDeleted == false &&
            m.SenderId == request.RecipientUserId ||
            m.RecipienId == request.RecipientUserId && m.SenderDeleted == false &&
            m.SenderId == request.CurrentUserId
            ).OrderBy(m => m.MessageSent)
            .AsQueryable();
        //.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
        //.ToListAsync(cancellationToken);

        var unreadMessages = messages.Where(m => m.DateRead == null
                && m.RecipienId == request.CurrentUserId).ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }
        }

        return new MessagesList { Messages = await messages.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync() };
    }

}

