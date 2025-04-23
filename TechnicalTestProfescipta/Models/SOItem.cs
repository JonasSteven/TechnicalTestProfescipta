using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTestProfescipta.Models
{
    [Table("SO_ITEM")]
    public class SOItem
    {
        [Column("SO_ITEM_ID")]
        public long SOItemId { get; set; }
        [Column("SO_ORDER_ID")]
        public long SOOrderId { get; set; }
        [Column("ITEM_NAME")]
        public string ItemName { get; set; }
        [Column("QUANTITY")]
        public int Quantity { get; set; }
        [Column("PRICE")]
        public decimal Price { get; set; }

        public SOOrder Order { get; set; } // Navigation
    }
}
