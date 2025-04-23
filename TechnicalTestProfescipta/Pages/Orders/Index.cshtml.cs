using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TechnicalTestProfescipta.Data;
using TechnicalTestProfescipta.Models;

namespace TechnicalTestProfescipta.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<SOOrder> SOOrderList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchKeyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.SOOrders
                        .Include(o => o.Customer)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                query = query.Where(o => o.OrderNo.Contains(SearchKeyword) || o.Customer.CustomerName.Contains(SearchKeyword));
            }

            if (FilterDate != null)
            {
                query = query.Where(o => o.OrderDate == FilterDate);
            }

            SOOrderList = await query.ToListAsync();
        }

        public async Task<IActionResult> OnGetExportExcelAsync()
        {
            var query = _context.SOOrders
                        .Include(o => o.Customer)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                query = query.Where(o => o.OrderNo.Contains(SearchKeyword) || o.Customer.CustomerName.Contains(SearchKeyword));
            }

            if (FilterDate != null)
            {
                query = query.Where(o => o.OrderDate == FilterDate);
            }

            var soOrderList = await query.ToListAsync();

            // Membuat file Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sales Orders");

                // Menambahkan header
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "Sales Order";
                worksheet.Cells[1, 3].Value = "Order Date";
                worksheet.Cells[1, 4].Value = "Customer";
                worksheet.Cells[1, 5].Value = "Address";

                // Menambahkan data
                int row = 2;
                int no = 1;
                foreach (var order in soOrderList)
                {
                    worksheet.Cells[row, 1].Value = no++;
                    worksheet.Cells[row, 2].Value = order.OrderNo;
                    worksheet.Cells[row, 3].Value = order.OrderDate.ToString("d/M/yyyy");
                    worksheet.Cells[row, 4].Value = order.Customer?.CustomerName;
                    worksheet.Cells[row, 5].Value = order.Address;
                    row++;
                }

                // Mengatur lebar kolom otomatis
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Menghasilkan file Excel dalam format memory stream
                var fileContents = package.GetAsByteArray();

                // Mengembalikan file sebagai download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesOrders.xlsx");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int DeleteId)
        {
            var order = await _context.SOOrders.FirstOrDefaultAsync(x => x.SOOrderId == DeleteId);
            if (order != null)
            {
                var items = await _context.SOItems
                    .Where(i => i.SOOrderId == DeleteId)
                    .Select(i => new SOItem
                    {
                        SOItemId = i.SOItemId,
                        SOOrderId = i.SOOrderId,
                        ItemName = i.ItemName,
                        Quantity = i.Quantity,
                        Price = Convert.ToDecimal(i.Price) // pastikan dikonversi ke decimal
                    })
                    .ToListAsync();
                _context.SOItems.RemoveRange(items);

                _context.SOOrders.Remove(order);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Sales Order deleted successfully.";
            }

            return RedirectToPage();
        }
    }
}
