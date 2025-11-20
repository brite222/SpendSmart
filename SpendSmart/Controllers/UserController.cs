using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

public class UsersController : Controller
{
    private readonly SpendSmartDbContext _context;

    public UsersController(SpendSmartDbContext context)
    {
        _context = context;
    }

    // GET: /Users
    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users); // calls Index.cshtml
    }

    // GET: /Users/ViewExpenses/5
    public IActionResult ViewExpenses(int id)
    {
        var user = _context.Users
            .Include(u => u.Expenses)
            .FirstOrDefault(u => u.Id == id);

        if (user == null) return NotFound();

        return View(user); // calls ViewExpenses.cshtml
    }

    // GET: /Users/Create
    public IActionResult Create()
    {
        return View(); // calls Create.cshtml
    }

    // POST: /Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Users.Add(model);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // GET: /Users/Edit/5
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        return View(user); // calls Edit.cshtml
    }

    // POST: /Users/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userInDb = _context.Users.Find(model.Id);
        if (userInDb == null) return NotFound();

        userInDb.Name = model.Name;
        userInDb.Age = model.Age;
        userInDb.Address = model.Address;
        userInDb.Profession = model.Profession;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // GET: /Users/Delete/5
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        return View(user); // calls Delete.cshtml
    }

    // POST: /Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
