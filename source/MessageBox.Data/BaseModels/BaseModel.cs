using System;

namespace MessageBox.Data.BaseModels
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Used as primary key for all entities
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Determine the entity is active
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Determine the entity is deleted
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Created date of an entity
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Updated date of an entity in UTC time format
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
