//namespace Blog.Tests.Ratings.Queries;

//[Collection("QueryCollection")]
//public class GetRatingListByUserQueryHandlerTests
//{
//    private readonly BlogDbContext Context;
//    private readonly IMapper Mapper;

//    public GetRatingListByUserQueryHandlerTests(QueryTestFixture fixture)
//    {
//        Context = fixture.Context;
//        Mapper = fixture.Mapper;
//    }
//    [Fact]
//    public async void GetRatingsByUserQueryHandler_Success()
//    {
//        //Arrange
//        var handler = new GetRatingListByUserQueryHandler(Context, Mapper);

//        //Act
//        var result = await handler.Handle(new GetRatingListByUserQuery
//        {
//            UserId = BlogContextFactory.UserBId
//        }, CancellationToken.None);
//        //Asset
//        result.ShouldBeOfType<RatingList>();
//        result.Ratings.Count.ShouldBe(2);
//    }
//    [Fact]
//    public async void GetRatingsByUserQueryHandler_ThrowsOnEmptyUserId()
//    {
//        //Arrange
//        var handler = new GetRatingListByUserQueryHandler(Context, Mapper);

//        //Act

//        //Asset
//        await Assert.ThrowsAsync<NotFoundException>(async () =>
//              await handler.Handle(new GetRatingListByUserQuery
//              {
//                  UserId = Guid.Empty
//              }, CancellationToken.None));
//    }
//}
