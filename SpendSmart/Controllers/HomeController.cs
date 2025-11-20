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
    public IActionResult CreateEditExpense(int? id)
    {
        Expense model;

        if (id.HasValue)
        {
            model = _context.Expenses
                .Include(e => e.User)
                .FirstOrDefault(e => e.Id == id.Value);

            if (model == null) return NotFound();
        }
        else
        {
            model = new Expense();
        }

        // Populate user dropdown
        ViewBag.UserList = new SelectList(_context.Users.ToList(), "Id", "Name", model.UserId);

        return View(model);
    }

    // POST: Create or Edit Expense
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateEditExpenseForm(Expense model)
    {
        // Validate User selection
        if (model.UserId == 0)
        {
            ModelState.AddModelError("UserId", "Please select a user.");
        }

        if (!ModelState.IsValid)
        {
            // Re-populate dropdown and return view
            ViewBag.UserList = new SelectList(_context.Users.ToList(), "Id", "Name", model.UserId);
            return View("CreateEditExpense", model);
        }

        if (model.Id == 0)
        {
            // New Expense
            model.SerialNumber = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
            _context.Expenses.Add(model);
        }
        else
        {
            // Update existing Expense
            var existing = _context.Expenses.AsNoTracking().FirstOrDefault(e => e.Id == model.Id);
            if (existing == null) return NotFound();

            model.SerialNumber = existing.SerialNumber; // keep serial
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
