using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTestProfescipta.Models
{
    [Table("COM_CUSTOMER")]
    public class COMCustomer
    {
        [Column("COM_CUSTOMER_ID")]
        public int COMCustomerId { get; set; }
        [Column("CUSTOMER_NAME")]
        public string CustomerName { get; set; }

        public ICollection<SOOrder> Orders { get; set; }  // <-- Untuk WithMany
    }
}
