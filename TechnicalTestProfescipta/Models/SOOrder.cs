using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTestProfescipta.Models
{
    [Table("SO_ORDER")]
    public class SOOrder
    {
        [Column("SO_ORDER_ID")]
        public long SOOrderId { get; set; }
        [Column("ORDER_NO")]
        public string OrderNo { get; set; }
        [Column("ORDER_DATE")]
        public DateTime OrderDate { get; set; }
        [Column("COM_CUSTOMER_ID")]
        public int COMCustomerId { get; set; }  // <-- FK
        [Column("ADDRESS")]
        public string Address { get; set; }

        public COMCustomer Customer { get; set; }  // Navigation
        public ICollection<SOItem> Items { get; set; }
    }
}
