//namespace Blog.Tests.Articles.Commands;

//public class DeleteArticleCommandHandlerTest : TestCommandBase
//{
//    [Fact]
//    public async Task DeleteArticleCommandHandler_Success()
//    {
//        //Arrange
//        var handler = new DeleteArticleCommandHandler(Context);

//        //Act
//        await handler.Handle(new DeleteArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForDelete,
//            UserId = BlogContextFactory.UserAId,
//            Role = Role.User
//        }, CancellationToken.None);
//        //Assert
//        Assert.Null(await Context.Articles.SingleOrDefaultAsync(art => art.Id == BlogContextFactory.ArticleIdForDelete));
//    }
//    [Fact]
//    public async Task DeleteArticleCommandHandler_SuccessIfAdmit()
//    {
//        //Arrange
//        var handler = new DeleteArticleCommandHandler(Context);

//        //Act
//        await handler.Handle(new DeleteArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForDelete,
//            UserId = BlogContextFactory.UserBId,
//            Role = Role.Admin
//        }, CancellationToken.None);
//        //Assert
//        Assert.Null(await Context.Articles.SingleOrDefaultAsync(art => art.Id == BlogContextFactory.ArticleIdForDelete));
//    }
//    [Fact]
//    public async Task DeleteArticleCommandHandler_FailOnWrongId()
//    {
//        //Arrange
//        var handler = new DeleteArticleCommandHandler(Context);

//        //Act
        
//        //Assert
//        await Assert.ThrowsAsync<NotFoundException>(async () => 
//              await handler.Handle(
//                  new DeleteArticleCommand
//                  {
//                      Id = Guid.NewGuid(),
//                      UserId = BlogContextFactory.UserAId,
//                      Role = Role.User
//                  },CancellationToken.None));
//    }

//    [Fact]
//    public async Task DeleteArticleCommandHandler_FailOnWrongUserId()
//    {
//        //Arrange
//        var deleteHandler = new DeleteArticleCommandHandler(Context);
//        var createhandler = new CreateArticleCommandHandler(Context);
//        var articleId = await createhandler.Handle(
//            new CreateArticleCommand
//            {
//                Title = "Article Title",
//                UserId = BlogContextFactory.UserAId,
//                Content = "Content",
                
//            }, CancellationToken.None);
//        //Act

//        //Assert
//        await Assert.ThrowsAsync<NotRightsException>(async () =>
//              await deleteHandler.Handle(
//                  new DeleteArticleCommand
//                  {
//                      Id = articleId,
//                      UserId = BlogContextFactory.UserBId,
//                      Role = Role.User
//                  }, CancellationToken.None));
//    }
    

//}
