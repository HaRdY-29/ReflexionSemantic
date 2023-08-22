using MongoDB.Driver;
using ReflexionSemantic.Data;
using System.Linq.Expressions;

namespace ReflexionSemantic.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
     where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            var database = new MongoClient(clientSettings).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        private protected static string GetCollectionName(Type documentType)
        {
            return (documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName ?? string.Empty;
        }       

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }
        public virtual IEnumerable<TDocument> FilterBySortBy(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, object>> sortByDescExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filterExpression).SortByDescending(sortByDescExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<TDocument> FindBy(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filter).ToList();
        }
        public IEnumerable<TDocument> FindAndSortBy(FilterDefinition<TDocument> filter, SortDefinition<TDocument> sort, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filter).Project(projectExpression).Sort(sort).ToList();
        }

        public IEnumerable<TDocument> FindAndProjectionBy(FilterDefinition<TDocument> filter, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filter).Project(projectExpression).ToList();
        }

        public virtual Task<TDocument> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TDocument document, CancellationToken cancellationToken = default)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(List<TDocument> documents, CancellationToken cancellationToken = default)
        {
            _collection.InsertMany(documents);
        }

        public async Task<bool> UpdateManyAsync(FilterDefinition<TDocument> filterDefinition, UpdateDefinition<TDocument> updateDefinition, CancellationToken cancellationToken = default)
        {
            var result = await _collection.UpdateManyAsync(filterDefinition, updateDefinition);
            return result.IsAcknowledged;
        }

        public virtual async Task InsertManyAsync(List<TDocument> documents, CancellationToken cancellationToken = default)
        {
            await _collection.InsertManyAsync(documents);
        }
        public virtual async Task InsertManyRawAsync(IEnumerable<TDocument> documents, InsertManyOptions options)
        {
            await _collection.InsertManyAsync(documents, options);
        }
        public void ReplaceOne(TDocument document, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

        public virtual IEnumerable<T> DistinctBy<T>(string fieldName, Expression<Func<TDocument, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return _collection.DistinctAsync<T>(fieldName, filterExpression).GetAwaiter().GetResult().ToEnumerable();
        }
        public virtual IEnumerable<TDocument> FilterByProjection(Expression<Func<TDocument, bool>> filterExpression, ProjectionDefinition<TDocument, TDocument> projectExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filterExpression).Project(projectExpression).ToList();
        }
        public virtual List<TDocument> FilterBySortByUsingFilterBuilder(FilterDefinition<TDocument> filters, SortDefinition<TDocument> sortByDescExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(filters).Sort(sortByDescExpression).ToList();
        }
        public virtual IAggregateFluent<TDocument> CustomAggregate(FilterDefinition<TDocument> filters, SortDefinition<TDocument> sortByDescExpression, CancellationToken cancellationToken = default)
        {
            return _collection.Aggregate(new AggregateOptions { AllowDiskUse = true }).Match(filters).Sort(sortByDescExpression);
        }
        public virtual IAggregateFluent<TDocument> CustomAggregate(
         FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default)
        {
            return _collection.Aggregate(new AggregateOptions { AllowDiskUse = true }).Match(filters);
        }
        public virtual long CountDocuments(
         FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default)
        {
            return _collection.CountDocuments(filters);
        }

        public virtual async Task<IAggregateFluent<TDocument>> CustomAggregateAsync(
       FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default)
        {
            var result = _collection.Aggregate(new AggregateOptions { AllowDiskUse = true }).Match(filters);
            return await Task.FromResult(result);
        }
    }
}
