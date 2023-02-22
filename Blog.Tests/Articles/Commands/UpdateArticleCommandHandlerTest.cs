//namespace Blog.Tests.Articles.Commands;

//public class UpdateArticleCommandHandlerTest : TestCommandBase
//{
//    [Fact]
//    public async Task UpdateArticleCommandHandler_Success()
//    {
//        //Arrange
//        var handler = new UpdateArticleCommandHandler(Context);
//        var updatedTitle = "new Title";
//        var updatedContent = "new content";


//        //Act
//        await handler.Handle(new UpdateArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForUpdate,
//            UserId = BlogContextFactory.UserBId,
//            Title = updatedTitle,
//            Content = updatedContent
//        }, CancellationToken.None);
//        //Assert
//        Assert.NotNull(await Context.Articles.SingleOrDefaultAsync(art =>
//            art.Id == BlogContextFactory.ArticleIdForUpdate
//            && art.Title == updatedTitle));
//    }
//    [Fact]
//    public async Task UpdateArticleCommandHandler_FailOnWrongId()
//    {
//        //Arrange
//        var handler = new UpdateArticleCommandHandler(Context);
        
//        //Act
        
//        //Assert
//       await Assert.ThrowsAsync<NotFoundException>(async () => 
//            await handler.Handle(
//                new UpdateArticleCommand
//                {
//                    Id = Guid.NewGuid(),
//                    UserId = BlogContextFactory.UserAId,
//                },CancellationToken.None));
//    }

//    [Fact]
//    public async Task UpdateArticleCommandHandler_FailOnWrongUserId()
//    {
//        //Arrange
//        var handler = new UpdateArticleCommandHandler(Context);

//        //Act

//        //Assert
//        await Assert.ThrowsAsync<NotRightsException>(async () =>
//             await handler.Handle(
//                 new UpdateArticleCommand
//                 {
//                     Id = BlogContextFactory.ArticleIdForUpdate,
//                     UserId = BlogContextFactory.UserAId,
//                 }, CancellationToken.None));
//    }

//}
