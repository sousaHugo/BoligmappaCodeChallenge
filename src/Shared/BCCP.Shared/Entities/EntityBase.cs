using System.ComponentModel.DataAnnotations;

namespace BCCP.Shared.Entities
{
    /// <summary>
    /// This class allows adding Identity and auditing data to database entities.
    /// </summary>
    public abstract class EntityBase
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
