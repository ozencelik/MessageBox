using System;

namespace MessageBox.Data.BaseEntities
{
    public abstract class BaseMySqlEntity : BaseEntity
    {
        /// <summary>
        /// Used as primary key for all entities
        /// </summary>
        public int Id { get; set; }
    }
}
