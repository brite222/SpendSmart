using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

public class HomeController : Controller
{
    private readonly SpendSmartDbContext _context;

    public HomeController(SpendSmartDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    // List all expenses
    public IActionResult Expenses()
    {
        var expenses = _context.Expenses
            .Include(e => e.User)
            .ToList();

        ViewBag.TotalExpenses = expenses.Sum(e => e.Value);

        return View(expenses);
    }

    // GET: Create or Edit Expense
    public IActionResult CreateEditExpense(int id = 0)
    {
        ViewBag.UserList = new SelectList(_context.Users.ToList(), "Id", "Name");

        if (id == 0)
            return View(new Expense());

        var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null) return NotFound();

        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateEditExpense(Expense model)
    {
        // Ensure server-side defaults for required fields that aren't on the form,
        // and remove any stale ModelState entries so validation uses the updated values.
        if (string.IsNullOrWhiteSpace(model.SerialNumber))
        {
            model.SerialNumber = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
            ModelState.Remove(nameof(model.SerialNumber));
        }

        if (string.IsNullOrWhiteSpace(model.Color))
        {
            model.Color = "Unknown";
            ModelState.Remove(nameof(model.Color));
        }

        if (string.IsNullOrWhiteSpace(model.Size))
        {
            model.Size = "OneSize";
            ModelState.Remove(nameof(model.Size));
        }

        // Re-populate dropdown and return if validation fails
        if (!ModelState.IsValid)
        {
            ViewBag.UserList = new SelectList(_context.Users.ToList(), "Id", "Name", model.UserId);
            return View(model);
        }

        if (model.Id == 0)
        {
            _context.Expenses.Add(model);
        }
        else
        {
            var existing = _context.Expenses.AsNoTracking().FirstOrDefault(e => e.Id == model.Id);
            if (existing == null) return NotFound();

            model.SerialNumber = existing.SerialNumber;
            _context.Expenses.Update(model);
        }

        _context.SaveChanges();
        return RedirectToAction("Expenses");
    }


    // Delete Expense
    public IActionResult DeleteExpense(int id)
    {
        var expense = _context.Expenses.Find(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }
        return RedirectToAction("Expenses");
    }

    public IActionResult Privacy() => View();
}
