namespace Blog.Tests.Common;

public class BlogContextFactory//create test data for testing
{    
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid ArticleIdForDelete = Guid.NewGuid();
    public static Guid ArticleIdForUpdate = Guid.NewGuid();

    public static int CommentIdForDelete = 7;
    public static int CommentIdForUpdate = 6;

    public static BlogDbContext Create()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlogDbContext(options);//create my own context with in memory database
        context.Database.EnsureCreated();
        context.Articles.AddRange(
            new Article
            {
                Id = Guid.Parse("9CD280D2-8C69-404B-A8A2-548DC77949AA"),
                CreatedBy = UserAId,
                UpdatedBy = null,
                CreatedTime = DateTime.Today,
                UpdatedTime = null,
                Title = "Title1",
                Content = "Content1",
                State = State.Approved,
                UserId = UserAId 
            },
            new Article
            {
                Id = Guid.Parse("CECFD3F5-F56E-4A83-A460-3520FA455E0A"),
                CreatedBy = UserBId,
                UpdatedBy = null,
                CreatedTime = DateTime.Today,
                UpdatedTime = null,
                Title = "Title2",
                Content = "Content2",
                State = State.Approved,
                UserId = UserBId
            },
            new Article
            {
                Id = ArticleIdForDelete,
                CreatedBy = UserAId,
                UpdatedBy = null,
                CreatedTime = DateTime.Today,
                UpdatedTime = null,
                Title = "Title3",
                Content = "Content3",
                State = State.Approved,
                UserId = UserAId
            },
            new Article
            {
                Id = ArticleIdForUpdate,
                CreatedBy = UserBId,
                UpdatedBy = null,
                CreatedTime = DateTime.Today,
                UpdatedTime = null,
                Title = "Title4",
                Content = "Content4",
                State = State.Waiting,
                UserId = UserBId
            }
        );
        context.Comments.AddRange(
            new Comment
            {
                ArticleId = ArticleIdForDelete,
                UserId = UserAId,
                Message = "Good article1",
                
            },
             new Comment
             {
                 ArticleId = ArticleIdForDelete,
                 UserId = UserAId,
                 Message = "Good article2",

             },
              new Comment
              {
                  ArticleId = ArticleIdForUpdate,
                  UserId = UserBId,
                  Message = "Good article3",

              },
               new Comment
               {
                   Id = CommentIdForUpdate,
                   ArticleId = ArticleIdForUpdate,
                   UserId = UserBId,
                   Message = "Good article4",

               },
                new Comment
                {
                    Id = CommentIdForDelete,
                    ArticleId = ArticleIdForDelete,
                    UserId = UserAId,
                    Message = "Good article5",

                }
            );
        context.Ratings.AddRange(
            new Rating
            {
                UserId = UserAId,
                ArticleId = ArticleIdForUpdate,
                Score = 4
            },
            new Rating
            {
                UserId = UserBId,
                ArticleId = ArticleIdForUpdate,
                Score = 5
            },
             new Rating
             {
                 UserId = UserBId,
                 ArticleId = ArticleIdForDelete,
                 Score = 1
             }

            );
        context.SaveChanges();
        return context;
    }
    public static void Destroy(BlogDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
