namespace Blog.Tests.Articles.Queries;

[Collection("QueryCollection")]
public class GetArticleContentHandlerTests
{
    private readonly BlogDbContext Context;
    private readonly IMapper Mapper;

    public GetArticleContentHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async void GetArticleContentQueryHandler_Success()
    {
        //Arrange
        var handler = new GetArticleDetailsQueryHandler(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetArticleContentQuery
        {
            Id = Guid.Parse("CECFD3F5-F56E-4A83-A460-3520FA455E0A")
        }, CancellationToken.None); 
        //Asset
        result.ShouldBeOfType<ArticleContentVm>();
        result.Title.ShouldBe("Title2");
        result.CreatedTime.ShouldBe(DateTime.Today);
    }

    [Fact]
    public async void GetArticleContentQueryHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new GetArticleDetailsQueryHandler(Context, Mapper);

        //Act
       
        //Asset
        await Assert.ThrowsAsync<NotFoundException>(async () =>
              await handler.Handle(new GetArticleContentQuery
              {
                  Id = Guid.Empty
              }, CancellationToken.None));
    }
}
