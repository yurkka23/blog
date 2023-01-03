//namespace Blog.Tests.Articles.Commands;

//public class VerifyArticleCommandHandlerTest : TestCommandBase
//{
//    [Fact]
//    public async Task VerifyArticleCommandHandler_Success()
//    {
//        //Arrange
//        var handler = new VerifyArticleCommandHandler(Context);
        

//        //Act
//        await handler.Handle(new VerifyArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForUpdate,
//           // UserId = BlogContextFactory.UserBId,
//            Role = Role.Admin,
//            State = State.Approved
//        }, CancellationToken.None);
//        //Assert
//        Assert.NotNull(await Context.Articles.SingleOrDefaultAsync(art =>
//            art.Id == BlogContextFactory.ArticleIdForUpdate
//            && art.State == State.Approved));
//    }

//    [Fact]
//    public async Task VerifyArticleCommandHandler_Success_Declined()
//    {
//        //Arrange
//        var handler = new VerifyArticleCommandHandler(Context);

//        //Act
//        await handler.Handle(new VerifyArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForDelete,
//            //UserId = BlogContextFactory.UserAId,
//            Role = Role.Admin,
//            State = State.Declined
//        }, CancellationToken.None);

//        //Assert
//        Assert.NotNull(await Context.Articles.SingleOrDefaultAsync(art =>
//            art.Id == BlogContextFactory.ArticleIdForDelete
//            && art.State == State.Declined));
//    }
//    [Fact]
//    public async Task VerifyArticleCommandHandler_Success_Waiting()
//    {
//        //Arrange
//        var handler = new VerifyArticleCommandHandler(Context);

//        //Act
//        await handler.Handle(new VerifyArticleCommand
//        {
//            Id = BlogContextFactory.ArticleIdForDelete,
//            //UserId = BlogContextFactory.UserAId,
//            Role = Role.Admin,
//            State = State.Waiting
//        }, CancellationToken.None);

//        //Assert
//        Assert.NotNull(await Context.Articles.SingleOrDefaultAsync(art =>
//            art.Id == BlogContextFactory.ArticleIdForDelete
//            && art.State == State.Waiting));
//    }

//    [Fact]
//    public async Task VerifyArticleCommandHandler_FailOn_RoleUser()
//    {
//        //Arrange
//        var handler = new VerifyArticleCommandHandler(Context);

//        //Act

//        //Assert
//        await Assert.ThrowsAsync<NotRightsException>(async () =>
//              await handler.Handle(new VerifyArticleCommand
//              {
//                  Id = BlogContextFactory.ArticleIdForUpdate,
//                  //UserId = BlogContextFactory.UserBId,
//                  Role = Role.User,
//                  State = State.Approved
//              }, CancellationToken.None));
//    }
//    [Fact]
//    public async Task VerifyArticleCommandHandler_FailOn_WrongId()
//    {
//        //Arrange
//        var handler = new VerifyArticleCommandHandler(Context);

//        //Act

//        //Assert
//        await Assert.ThrowsAsync<NotFoundException>(async () =>
//              await handler.Handle(new VerifyArticleCommand
//              {
//                  Id = Guid.Empty,
//                  //UserId = BlogContextFactory.UserBId,
//                  Role = Role.Admin,
//                  State = State.Approved
//              }, CancellationToken.None));
//    }
//}
