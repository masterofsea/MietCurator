using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MietCurator.DAL.Contexts.Miet
{
    [Table("groups")]
    public class Groups
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(8)")]
        [Required]
        public string Name { get; set; }
    }
}