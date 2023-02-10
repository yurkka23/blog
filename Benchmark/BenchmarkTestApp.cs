using AutoMapper;
using BenchmarkDotNet.Attributes;
using Blog.Application;
using Blog.Application.Articles.Commands.CreateArticle;
using Blog.Application.Articles.Queries.GetArticeGenres;
using Blog.Application.Articles.Queries.GetArticleContent;
using Blog.Application.Articles.Queries.GetArticleList;
using Blog.Application.Common.Mappings;
using Blog.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
namespace Benchmark;

[RankColumn]
[MemoryDiagnoser]
public class BenchmarkTestApp
{
    private readonly IMapper Mapper;
    private readonly IOptions<MongoEntitiesDBSettings> _optionsEntitites;
    private readonly IOptions<MongoUserDBSettings> _optionsUsers;
    private readonly string accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjJjZDJjYzNlLTYyMTAtNDAzNS1hNWYwLTkyNWIwNDVjNTBiOCIsImV4cCI6MTY3NTk0Nzk1MX0.WWD08n2J48tfCGfVEVcLZbHT9N9rNjEfFPj0mgcIw8SNzVQs9FrlhXCcZ5afSBIVjguFRgm3f7dusJknTaIFFQ";

    public BenchmarkTestApp()
    {

        var configurationBuider = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AssemblyMappingProfile(typeof(AssemblyMappingProfile).Assembly));
                });
        Mapper = configurationBuider.CreateMapper();

        MongoEntitiesDBSettings mongoEntitiesDBSettings = new MongoEntitiesDBSettings()
        {
            CollectionName = "EntitiesBlogStore",
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "StoreBlog"
        };
        MongoUserDBSettings mongoUsersDBSettings = new MongoUserDBSettings()
        {
            CollectionName = "UserBlogStore",
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "StoreBlog"
        };
        _optionsEntitites = Options.Create(mongoEntitiesDBSettings);
        _optionsUsers = Options.Create(mongoUsersDBSettings);
    }

    //[Benchmark]
    //public async Task GetArticleGenresQueryHandler()
    //{

    //    var handler = new GetArticleGenresQueryHandler(_optionsEntitites, Mapper);

    //    var genreList = await handler.Handle(
    //        new GetArticleGenresQuery
    //        {
    //            CountGenres = 14
    //        },
    //        CancellationToken.None);
    //}

    //[Benchmark]
    //public async Task GetArticleDetailsQueryHandler()
    //{
    //    var handler = new GetArticleDetailsQueryHandler(Mapper,_optionsEntitites,_optionsUsers);

    //    var article = await handler.Handle(
    //        new GetArticleContentQuery
    //        {
    //            Id = Guid.Parse("d86c04ea-887c-4904-b66c-aedc6e271568"),   
    //            UserId = Guid.Parse("7299cc84-d09c-4d8d-bf41-c5a758c06fe3")
    //        },
    //        CancellationToken.None);
    //}

    [Benchmark]
    public async Task GetArticleListByGenre()
    {
        using var httpClient = new HttpClient();

        string genre = "string";

        using var response = await httpClient.GetAsync($"https://localhost:7218/article/get-articles-by-genres?genre={genre}");
    }

    [Benchmark]
    public async Task SearchArticleByTitle()
    {
        using var httpClient = new HttpClient();

        string partTitle = "how to";

        using var response = await httpClient.GetAsync($"https://localhost:7218/article/search-articles-by-title?partTitle={partTitle}");
    }
    [Benchmark]
    public async Task SearchWatingArticleByTitle()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string partTitle = "h";

        using var response = await httpClient
            .GetAsync($"https://localhost:7218/article/search-waiting-articles-by-title?partTitle={partTitle}");
    }

    [Benchmark]
    public async Task GetArticlesByUser()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string userId = "2cd2cc3e-6210-4035-a5f0-925b045c50b8";

        using var response = await httpClient
            .GetAsync($"https://localhost:7218/article/get-another-user-articles?Id={userId}");
    }
    [Benchmark]
    public async Task GetArticleContent()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string actilceId = "5589df12-341a-4dda-bcca-499822689716";

        using var response = await httpClient
            .GetAsync($"https://localhost:7218/article/get-article-content-by-id?id={actilceId}");
    }

    [Benchmark]
    public async Task SearchUsersByUserName()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        string username = "aa";

        using var response = await httpClient
            .GetAsync($"https://localhost:7218/user/search-users-by-username?partUsername={username}");
    }
}
