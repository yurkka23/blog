﻿using System;
using MediatR;

namespace Blog.Application.Ratings.Commands.CreateRating;

public class CreateRatingCommand : IRequest<int>
{
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public byte Score { get; set; } 
}
