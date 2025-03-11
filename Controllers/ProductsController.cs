using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _39eertime_.Data;
using _39eertime_.Models;

namespace _39eertime_.Controllers
{
    public class ProductsController : Controller
    {
        private readonly _39eertime_Context _context;

        public ProductsController(_39eertime_Context context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, string searchOption)
        {
            // Pass the search string and option to the view
            ViewData["CurrentFilter"] = searchString;
            ViewData["SearchOption"] = searchOption;

            // Get all products from the database
            var products = from p in _context.Product
                           select p;

            // Filter products based on the selected search option
            if (!string.IsNullOrEmpty(searchString))
            {
                switch (searchOption)
                {
                    case "Name":
                        products = products.Where(p => p.Name.Contains(searchString));
                        break;

                    case "Distributor":
                        products = products.Where(p => p.Distributor.Contains(searchString));
                        break;
                    case "Quantity_Available":
                        if (int.TryParse(searchString, out int quantity))

                        {
                            products = products.Where(p => p.Quantity_Available == quantity);

                        }
                        break;
                    default:
                        // No filtering if no valid option is selected
                        break;
                }
            }

            // Return the filtered list of products to the view
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Distributor,Quantity_Available,Monthly_Amount")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Calculate Quantity_Required before saving
                product.CalculateQuantityRequired();

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Distributor,Quantity_Available,Monthly_Amount")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate Quantity_Required before updating
                    product.CalculateQuantityRequired();

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}