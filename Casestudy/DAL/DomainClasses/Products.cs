using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Casestudy.DAL.DomainClasses
{
    public class Products
    {
        [Key]
        [StringLength(15)]
        public string? id { get; set; }

        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brands? Brands { get; set; } // generates FK
        [Required]

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]

        public byte[]? Timer { get; set; }

        [Required]
        [StringLength(50)]
        public string? ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string? GraphicName { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }

        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOnBackOrder { get; set; }

        
        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
