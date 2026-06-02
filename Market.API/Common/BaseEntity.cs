using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Common
{
    /// <summary>
    /// Base entity with soft delete support and GUID as primary key
    /// </summary>
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Soft delete flag - entity is not physically deleted
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Timestamp when entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when entity was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Timestamp when entity was soft deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// User who created the entity
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the entity
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Soft delete the entity
        /// </summary>
        public void Delete(string? deletedBy = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedBy = deletedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Restore a soft-deleted entity
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
