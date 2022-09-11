global using Xunit;
global using AutoMapper;
global using Blog.Application.Articles.Queries.GetArticleContent;
global using Blog.Application.Common.Exceptions;
global using Blog.Persistence.EntityContext;
global using Blog.Tests.Common;
global using Microsoft.EntityFrameworkCore;
global using Blog.Domain.Enums;
global using Shouldly;
global using Blog.Application.Articles.Queries.GetArticleList;
global using Blog.Application.Articles.Queries.GetArticlesByUser;
global using System;
global using System.Threading.Tasks;
global using Blog.Application.Comments.Commands.CreateComment;
global using Blog.Application.Common.Mappings;
global using Blog.Application.Interfaces;
global using Blog.Application.Ratings.Queries;
global using Blog.Application.Ratings.Queries.GetRatingListByUser;
global using Blog.Application.Ratings.Queries.GetRatingByArticle;
global using Blog.Application.Ratings.Queries.GetRatingListByArticle;
global using Blog.Domain.Models;
global using Blog.Application.Ratings.Commands.CreateRating;
global using Blog.Application.Articles.Commands.DeleteArticle;
global using Blog.Application.Articles.Commands.CreateArticle;
global using Blog.Application.Articles.Commands.UpdateArticle;
global using Blog.Application.Articles.Commands.VerifyArticle;
global using Blog.Application.Comments.Queries.GetCommentsByArticle;
global using Blog.Application.Comments.Commands.DeleteComment;
global using Blog.Application.Comments.Commands.UpdateComment;
