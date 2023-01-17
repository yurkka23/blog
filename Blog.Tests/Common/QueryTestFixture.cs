//namespace Blog.Tests.Common;

////helping class
//public class QueryTestFixture : IDisposable
//{
//    public BlogDbContext Context;
//    public IMapper Mapper;
//    public QueryTestFixture()
//    {
//        Context = BlogContextFactory.Create();
//        var configurationBuider = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new AssemblyMappingProfile(typeof(IBlogDbContext).Assembly));
//        });
//        Mapper = configurationBuider.CreateMapper();
//    }
//    public void Dispose()
//    {
//        BlogContextFactory.Destroy(Context);    
//    }
//}
//[CollectionDefinition("QueryCollection")]
//public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
