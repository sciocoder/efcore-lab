// https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#primitive-collections

using Microsoft.EntityFrameworkCore;

await using var db = new DemoContext();
db.Database.EnsureCreated();


var queryableWithConcrete = db.Posts.Where(p => p.TagsConcrete.Contains("word"));
// var queryWithConcrete = queryableWithConcrete.ToQueryString();
/*
SELECT [p].[PostId], [p].[Content], [p].[TagsAbstract], [p].[TagsConcrete], [p].[Title]
FROM [Posts] AS [p]
WHERE N'word' IN (
    SELECT [t].[value]
    FROM OPENJSON([p].[TagsConcrete]) WITH ([value] nvarchar(max) '$') AS [t]
)
*/
// var queryProjectionWithConcrete = queryableWithConcrete
//     .Select(p => new { p.PostId, p.Title, p.Content, TagsList = p.TagsConcrete })
//     .ToQueryString();
/*
SELECT [p].[PostId], [p].[Title], [p].[Content], [p].[TagsConcrete] AS [TagsList]
 FROM [Posts] AS [p]
 WHERE N'word' IN (
     SELECT [t].[value]
     FROM OPENJSON([p].[TagsConcrete]) WITH ([value] nvarchar(max) '$') AS [t]
 )
*/
var queryProjectionToModelWithConcrete = queryableWithConcrete
    .Select(p => new PostModel(p.PostId, p.Title, p.Content, p.TagsConcrete.ToArray()))
    .ToQueryString();
/*
SELECT [p].[PostId], [p].[Title], [p].[Content], [p].[TagsConcrete]
FROM [Posts] AS [p]
WHERE N'word' IN (
    SELECT [t].[value]
    FROM OPENJSON([p].[TagsConcrete]) WITH ([value] nvarchar(max) '$') AS [t]
)

RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor._selectExpression
SELECT p.PostId, p.Title, p.Content, p.TagsConcrete
FROM Posts AS p
WHERE N'word' IN (
    SELECT t.value
    FROM OPENJSON(p.TagsConcrete) WITH (value nvarchar(max) '') AS t)
*/

var queryableWithAbstract = db.Posts.Where(p => p.TagsAbstract.Contains("word"));
//var queryWithAbstract = queryableWithAbstract.ToQueryString();
/*
SELECT [p].[PostId], [p].[Content], [p].[TagsAbstract], [p].[TagsConcrete], [p].[Title]
FROM [Posts] AS [p]
WHERE N'word' IN (
    SELECT [t].[value]
    FROM OPENJSON([p].[TagsAbstract]) WITH ([value] nvarchar(max) '$') AS [t]
)
*/
// var queryProjectionWithAbstract = queryableWithAbstract
//     .Select(p => new { p.PostId, p.Title, p.Content, TagsIList = p.TagsAbstract })
//     .ToQueryString();
/*
SELECT [p].[PostId], [p].[Title], [p].[Content], [p].[TagsAbstract] AS [TagsIList]
FROM [Posts] AS [p]
WHERE N'word' IN (
    SELECT [t].[value]
    FROM OPENJSON([p].[TagsAbstract]) WITH ([value] nvarchar(max) '$') AS [t]
)
*/
var queryProjectionToModelWithAbstract = queryableWithAbstract
    .Select(p => new PostModel(p.PostId, p.Title, p.Content, p.TagsAbstract.ToArray()))
    .ToQueryString();
/*
System.NullReferenceException: Object reference not set to an instance of an object.
at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.CreateGetValueExpression
=> var getMethod = typeMapping.GetDataReaderMethod();  typeMapping (RelationalTypeMapping) is null


RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor._selectExpression
   SELECT p.PostId, p.Title, p.Content, t0.value, t0.key
   FROM Posts AS p
   OUTER APPLY OPENJSON(p.TagsAbstract) AS t0
   WHERE N'word' IN (
       SELECT t.value
       FROM OPENJSON(p.TagsAbstract) WITH (value nvarchar(max) '') AS t)
   ORDER BY p.PostId ASC, CAST(t0.key AS int) ASC


Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor
    _queryTranslationPreprocessorFactory.Create(this).Process(query)
        NavigationExpandingExpressionVisitor(...).Expand(query)


Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.CreateGetValueExpression(ParameterExpression dbDataReader, Int32 index, Boolean nullable, RelationalTypeMapping typeMapping, Type type, IPropertyBase property)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.VisitExtension(Expression extensionExpression)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.ProcessShaper(Expression shaperExpression, RelationalCommandCache& relationalCommandCache, IReadOnlyList`1& readerColumns, LambdaExpression& relatedDataLoaders, Int32& collectionId)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.VisitExtension(Expression extensionExpression)
   at System.Dynamic.Utils.ExpressionVisitorUtils.VisitArguments(ExpressionVisitor visitor, IArgumentProvider nodes)
   at System.Linq.Expressions.ExpressionVisitor.VisitMethodCall(MethodCallExpression node)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.VisitMethodCall(MethodCallExpression methodCallExpression)
   at System.Dynamic.Utils.ExpressionVisitorUtils.VisitArguments(ExpressionVisitor visitor, IArgumentProvider nodes)
   at System.Linq.Expressions.ExpressionVisitor.VisitNew(NewExpression node)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.ShaperProcessingExpressionVisitor.ProcessShaper(Expression shaperExpression, RelationalCommandCache& relationalCommandCache, IReadOnlyList`1& readerColumns, LambdaExpression& relatedDataLoaders, Int32& collectionId)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.VisitShapedQuery(ShapedQueryExpression shapedQueryExpression)
   at Microsoft.EntityFrameworkCore.Query.ShapedQueryCompilingExpressionVisitor.VisitExtension(Expression extensionExpression)
   at Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.VisitExtension(Expression extensionExpression)
   at Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor[TResult](Expression query)
   at Microsoft.EntityFrameworkCore.Storage.Database.CompileQuery[TResult](Expression query, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.CompileQueryCore[TResult](IDatabase database, Expression query, IModel model, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.<>c__DisplayClass9_0`1.<Execute>b__0()
   at Microsoft.EntityFrameworkCore.Query.Internal.CompiledQueryCache.GetOrAddQuery[TResult](Object cacheKey, Func`1 compiler)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.Execute[TResult](Expression query)
   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.Execute[TResult](Expression expression)
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToQueryString(IQueryable source)
   at Program.<Main>$(String[] args) in Program.cs:line 70
   at Program.<Main>$(String[] args) in Program.cs:line 70
   at Program.<Main>(String[] args)
*/


public class DemoContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=efcore.demo;User Id=sa;Password=Password!;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>().HasData(
            Enumerable.Range(1, 5).Select(
                index => new Post
                {
                    PostId = index,
                    Title = $"Title{index}",
                    Content = $"Content{index}",
                    TagsAbstract = [$"Tags{index}-1", $"Tags{index}-2", $"Tags{index}-3"],
                    TagsConcrete = [$"Tags{index}-1", $"Tags{index}-2", $"Tags{index}-3"]
                }));
    }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public List<string> TagsConcrete { get; set; }
    public IList<string> TagsAbstract { get; set; }
}

public record PostModel(int PostId, string Title, string Content, string[] Tags);
