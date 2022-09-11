namespace Blog.Tests.Articles.Queries;

[Collection("QueryCollection")]
public class GetArticleListQueryHandlerTests
{
    private readonly BlogDbContext Context;
    private readonly IMapper Mapper;

    public GetArticleListQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async void GetArticleListQueryHandler_Success()
    {
        //Arrange
        var handler = new GetArticleListQueryHandle(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetArticleListQuery
        {
            State = State.Approved
        }, CancellationToken.None);
        //Asset
        result.ShouldBeOfType<ArticleListVm>();
        result.Articles.Count.ShouldBe(3);
    }
    [Fact]
    public async void GetArticleListQueryHandler_SuccessWaiting()
    {
        //Arrange
        var handler = new GetArticleListQueryHandle(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetArticleListQuery
        {
            State = State.Waiting
        }, CancellationToken.None);
        //Asset
        result.ShouldBeOfType<ArticleListVm>();
        result.Articles.Count.ShouldBe(1);
    }


}
