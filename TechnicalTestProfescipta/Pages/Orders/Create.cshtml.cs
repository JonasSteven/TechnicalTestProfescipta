using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TechnicalTestProfescipta.Data;
using TechnicalTestProfescipta.Models;

namespace TechnicalTestProfescipta.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public List<COMCustomer> Customers { get; set; }

        // Model list untuk detail item
        [BindProperty]
        public List<string> ItemNames { get; set; } = new List<string>();

        [BindProperty]
        public List<int> Quantities { get; set; } = new List<int>();

        [BindProperty]
        public List<decimal> Prices { get; set; } = new List<decimal>();

        public async Task OnGetAsync()
        {
            // Load customers or other data if needed
            Customers = await _context.COMCustomers.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string orderNo, int comCustomerId, DateTime orderDate, string address)
        {
            // Validasi form secara manual
            if (string.IsNullOrEmpty(orderNo) || comCustomerId == 0 || orderDate == default)
            {
                // Jika data tidak valid, reload customers dan tampilkan halaman kembali
                Customers = await _context.COMCustomers.ToListAsync();
                ModelState.AddModelError("", "Please fill in all required fields.");
                return Page();
            }

            // Membuat objek SOOrder secara manual
            var salesOrder = new SOOrder
            {
                OrderNo = orderNo,
                COMCustomerId = comCustomerId,
                OrderDate = orderDate,
                Address = string.IsNullOrEmpty(address) ? "" : address
            };

            // Insert SO_ORDER
            _context.SOOrders.Add(salesOrder);
            await _context.SaveChangesAsync();

            // Insert SO_ITEM
            for (int i = 0; i < ItemNames.Count; i++)
            {
                var soItem = new SOItem
                {
                    SOOrderId = salesOrder.SOOrderId,  // Ambil SO_ORDER_ID yang baru diinsert
                    ItemName = ItemNames[i],
                    Quantity = Quantities[i],
                    Price = Prices[i]
                };
                _context.SOItems.Add(soItem);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sales Order created successfully!";

            return RedirectToPage("./Index");
        }
    }

}
