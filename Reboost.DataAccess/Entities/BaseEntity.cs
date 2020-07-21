using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reboost.DataAccess.Entities
{
    /// <summary>
    /// Base entity contains general properties that other entities derive from.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Number that uniquely identifies a single record of the entity.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
    }
}
