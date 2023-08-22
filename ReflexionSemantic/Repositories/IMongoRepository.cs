using MongoDB.Driver;
using ReflexionSemantic.Data;
using System.Linq.Expressions;

namespace ReflexionSemantic.Repositories
{
    public interface IMongoRepository<TDocument>
                     where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);

        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression, CancellationToken cancellationToken = default);
        IEnumerable<TDocument> FilterBySortBy(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortByDescExpression, CancellationToken cancellationToken = default);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        TDocument FindById(string id, CancellationToken cancellationToken = default);
        Task<TDocument> FindByIdAsync(string id, CancellationToken cancellationToken = default);
        void InsertOne(TDocument document, CancellationToken cancellationToken = default);
        Task InsertOneAsync(TDocument document, CancellationToken cancellationToken = default);
        void InsertMany(List<TDocument> documents, CancellationToken cancellationToken = default);
        Task InsertManyRawAsync(IEnumerable<TDocument> documents, InsertManyOptions options);
        Task InsertManyAsync(List<TDocument> documents, CancellationToken cancellationToken = default);
        void ReplaceOne(TDocument document, CancellationToken cancellationToken = default);
        Task ReplaceOneAsync(TDocument document, CancellationToken cancellationToken = default);
        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        void DeleteById(string id, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        IEnumerable<TDocument> FindBy(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default);
        IEnumerable<TDocument> FindAndSortBy(FilterDefinition<TDocument> filter, SortDefinition<TDocument> sort, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default);
        IEnumerable<TDocument> FindAndProjectionBy(FilterDefinition<TDocument> filter, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default);
        Task<bool> UpdateManyAsync(FilterDefinition<TDocument> filterDefinition, UpdateDefinition<TDocument> updateDefinition, CancellationToken cancellationToken = default);
        IEnumerable<T> DistinctBy<T>(string fieldName, Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default);
        IEnumerable<TDocument> FilterByProjection(Expression<Func<TDocument, bool>> filterExpression, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default);
        List<TDocument> FilterBySortByUsingFilterBuilder(FilterDefinition<TDocument> filters, SortDefinition<TDocument> sortByDescExpression, CancellationToken cancellationToken = default);
        IAggregateFluent<TDocument> CustomAggregate(FilterDefinition<TDocument> filters, SortDefinition<TDocument> sortByDescExpression, CancellationToken cancellationToken = default);
        IAggregateFluent<TDocument> CustomAggregate(FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default);
        long CountDocuments(FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default);
        Task<IAggregateFluent<TDocument>> CustomAggregateAsync(FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default);
    }
}
