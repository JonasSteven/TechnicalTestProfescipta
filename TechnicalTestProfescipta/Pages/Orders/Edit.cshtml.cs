using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TechnicalTestProfescipta.Data;
using TechnicalTestProfescipta.Models;

namespace TechnicalTestProfescipta.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SOOrder SalesOrder { get; set; } = new SOOrder();

        [BindProperty]
        public List<string> ItemNames { get; set; } = new List<string>();

        [BindProperty]
        public List<int> Quantities { get; set; } = new List<int>();

        [BindProperty]
        public List<decimal> Prices { get; set; } = new List<decimal>();

        public List<COMCustomer> Customers { get; set; } = new List<COMCustomer>();

        public List<SOItem> ExistingItems { get; set; } = new List<SOItem>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            SalesOrder = await _context.SOOrders.FirstOrDefaultAsync(x => x.SOOrderId == id);
            if (SalesOrder == null)
            {
                return NotFound();
            }

            Customers = await _context.COMCustomers.ToListAsync();
            ExistingItems = await _context.SOItems
                 .Where(i => i.SOOrderId == id)
                 .Select(i => new SOItem
                 {
                     SOItemId = i.SOItemId,
                     SOOrderId = i.SOOrderId,
                     ItemName = i.ItemName,
                     Quantity = i.Quantity,
                     Price = Convert.ToDecimal(i.Price) // force convert
                 }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validasi manual
            if (string.IsNullOrEmpty(SalesOrder.OrderNo) || SalesOrder.COMCustomerId == 0 || SalesOrder.OrderDate == default)
            {
                Customers = await _context.COMCustomers.ToListAsync();
                ModelState.AddModelError("", "Please fill in all required fields.");
                return Page();
            }

            var soOrderId = SalesOrder.SOOrderId;

            // Update SOOrder
            var salesOrder = await _context.SOOrders.FirstOrDefaultAsync(x => x.SOOrderId == soOrderId);
            if (salesOrder == null)
                return NotFound();

            salesOrder.OrderNo = SalesOrder.OrderNo;
            salesOrder.COMCustomerId = SalesOrder.COMCustomerId;
            salesOrder.OrderDate = SalesOrder.OrderDate;
            salesOrder.Address = string.IsNullOrEmpty(SalesOrder.Address) ? "" : SalesOrder.Address;

            _context.Entry(salesOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Hapus semua item sebelumnya
            var existingItems = await _context.SOItems
                .Where(i => i.SOOrderId == soOrderId)
                .Select(i => new SOItem
                {
                    SOItemId = i.SOItemId,
                    SOOrderId = i.SOOrderId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    Price = Convert.ToDecimal(i.Price) // pastikan dikonversi ke decimal
                })
                .ToListAsync();
            _context.SOItems.RemoveRange(existingItems);
            await _context.SaveChangesAsync();

            // Tambah ulang item baru
            for (int i = 0; i < ItemNames.Count; i++)
            {
                var item = new SOItem
                {
                    SOOrderId = soOrderId,
                    ItemName = ItemNames[i],
                    Quantity = Quantities[i],
                    Price = Prices[i]
                };
                _context.SOItems.Add(item);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sales Order updated successfully!";

            return RedirectToPage("./Index");
        }
    }
}
