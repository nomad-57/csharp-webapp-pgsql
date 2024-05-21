using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simple_db_crud.Models;

namespace Simple_db_crud.Controllers
{
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;
        private readonly TodoDbContext _context;

        public TodoController(TodoDbContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Get All Entries");
            return _context.Todo != null ? 
                        View(await _context.Todo.ToListAsync()) :
                        Problem("Entity set 'TodoDbContext.Todo'  is null.");
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Get info about Entry" + id);
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Create a new Entry");
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Done,CreatedAt,LastUpdate")] Todo todo)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Create a new Entry with data" + todo);
            if (ModelState.IsValid)
            {
                // todo.CreatedAt = DateTime.UtcNow();
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Edit Entry" + id);
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Done,CreatedAt,LastUpdate")] Todo todo)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Edit entry" + id + "with data" + todo);
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var todoInDb = _context.Todo.AsNoTracking().Where(s => s.Id == id).FirstOrDefault();
                    if (todoInDb != null)
                    {   
                        todo.CreatedAt = todoInDb.CreatedAt;
                    }
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Delete Entry" + id);
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Confirm Delete of Entry" + id);
            if (_context.Todo == null)
            {
                return Problem("Entity set 'TodoDbContext.Todo'  is null.");
            }
            var todo = await _context.Todo.FindAsync(id);
            if (todo != null)
            {
                _context.Todo.Remove(todo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
          return (_context.Todo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            DateTime date = DateTime.Now;
            string dateWithFormat = date.ToString("s");
            _logger.LogInformation(dateWithFormat + " Error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
