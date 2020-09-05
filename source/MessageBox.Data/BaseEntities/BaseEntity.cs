using System;

namespace MessageBox.Data.BaseEntities
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Used as primary key for all entities
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Determine the entity is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Determine the entity is deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Created date of an entity
        /// </summary>
        public DateTime CreatedOn { get; protected set; } = DateTime.Now;

        /// <summary>
        /// Updated date of an entity in UTC time format
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
