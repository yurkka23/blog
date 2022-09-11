namespace Blog.Tests.Articles.Queries;

[Collection("QueryCollection")]
public class GetArticlesByUserQueryHandlerTests
{
    private readonly BlogDbContext Context;
    private readonly IMapper Mapper;

    public GetArticlesByUserQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async void GetArticlesByUserQueryHandler_Success()
    {
        //Arrange
        var handler = new GetArticlesByUserQueryHandle(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetArticlesByUserQuery
        {
            UserId = BlogContextFactory.UserAId
        }, CancellationToken.None); 
        //Asset
        result.ShouldBeOfType<ArticleListVm>();
        result.Articles?.Count.ShouldBe(2);
    }

    [Fact]
    public async void GetArticlesByUserQueryHandler_FailedOnWrongUserId()
    {
        //Arrange
        var handler = new GetArticlesByUserQueryHandle(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetArticlesByUserQuery
        {
            UserId = Guid.Parse("929FC892-D34E-4F1A-A17F-D2266F95E750")
        }, CancellationToken.None);
        //Asset
        result.ShouldBeOfType<ArticleListVm>();
        result.Articles?.Count.ShouldBe(0);
    }


}
