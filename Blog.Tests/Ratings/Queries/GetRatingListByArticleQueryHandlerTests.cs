namespace Blog.Tests.Ratings.Queries;

[Collection("QueryCollection")]
public class GetRatingListByArticleQueryHandlerTests
{
    private readonly BlogDbContext Context;
    private readonly IMapper Mapper;

    public GetRatingListByArticleQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }
    [Fact]
    public async void GetRatingsByAticleQueryHandler_Success()
    {
        //Arrange
        var handler = new GetRatingListByArticleQueryHandler(Context, Mapper);

        //Act
        var result = await handler.Handle(new GetRatingListByArticleQuery
        {
            ArticleId = BlogContextFactory.ArticleIdForUpdate
        }, CancellationToken.None);
        //Asset
        result.ShouldBeOfType<RatingListVm>();
        result.Ratings.Count.ShouldBe(2);
    }
    [Fact]
    public async void GetRatingsByAticleQueryHandler_ThrowsOnEmptyArticleId()
    {
        //Arrange
        var handler = new GetRatingListByArticleQueryHandler(Context, Mapper);

        //Act

        //Asset
        await Assert.ThrowsAsync<NotFoundException>(async () =>
              await handler.Handle(new GetRatingListByArticleQuery
              {
                  ArticleId = Guid.Empty
              }, CancellationToken.None));
    }
}
