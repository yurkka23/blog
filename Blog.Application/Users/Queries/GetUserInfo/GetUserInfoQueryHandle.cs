using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces;
using Blog.Application.Common.Exceptions;
using Blog.Domain.Models;
using AutoMapper;

namespace Blog.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandle : IRequestHandler<GetUserInfoQuery, UserInfoVm>
{
    private readonly IBlogDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetUserInfoQueryHandle(IBlogDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<UserInfoVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

        if (userQuery == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        return _mapper.Map<UserInfoVm>(userQuery);
    }
}
