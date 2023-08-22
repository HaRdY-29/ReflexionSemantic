using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReflexionSemantic.Data
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }

        DateTime? CreatedTs { get; set; }

        DateTime? ModifiedTs { get; set; }

        public bool? IsActive { get; set; }

        Guid? CreatedBy { get; set; }

        Guid? ModifiedBy { get; set; }
    }
}
