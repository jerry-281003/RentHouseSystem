using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RentHouseSystem.Data;
using RentHouseSystem.Models;

namespace RentHouseSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly RentHouseSystemContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HousesController(RentHouseSystemContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> DetailHouse(string id)
        {
            if (id == null || _context.House == null)
            {
                return NotFound();
            }

            var house = await _context.House
                 .FirstOrDefaultAsync(m => m.HouseId == id);
            if (house == null)
            {
                return NotFound();
            }
            return View(house);
        }
        public IActionResult Search(string keyword)
        {
            var result = _context.House
         .Include(h => h.Images) // Include related images
         .Where(t => EF.Functions.Like(t.title, $"%{keyword}%") ||
                     EF.Functions.Like(t.Description, $"%{keyword}%"))
         .Select(h => new HouseSearchModel
         {
             HouseId = h.HouseId,
             ownerId = h.ownerId,
             title = h.title,
             Address = h.Address,
             Description = h.Description,
             ContactPhone = h.ContactPhone,
             Province = h.Province,
             District = h.District,
             Ward = h.Ward,
             Status = h.Status,
             CreatedAt = h.CreatedAt,
             UpdatedAt = h.UpdatedAt,
             ImageUrls = h.Images.FirstOrDefault().ImageUrl // Fetch the first image URL
         })
         .ToList();
            return View(result);
        }
        // GET: Houses
        public async Task<IActionResult> Index()
        {
              return _context.House != null ? 
                          View(await _context.House.ToListAsync()) :
                          Problem("Entity set 'RentHouseSystemContext.House'  is null.");
        }

        // GET: Houses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.House == null)
            {
                return NotFound();
            }

            var house = await _context.House
               .Include(h => h.Images)
        .FirstOrDefaultAsync(h => h.HouseId == id); ;
            if (house == null)
            {
                return NotFound();
            }
            var viewModel = new HouseViewModel
            {
                HouseId=house.HouseId,
                ownerId=house.ownerId,
                title=house.title,
                Address=house.Address,
                Description=house.Description,
                ContactPhone=house.ContactPhone,
                Province=house.Province,
                District=house.District,
                Ward=house.Ward,
                Status=house.Status,
                CreatedAt=house .CreatedAt,
                UpdatedAt=house .UpdatedAt,
                ImageUrls = house.Images.Select(i => i.ImageUrl).ToList()
            };
            return View(viewModel);
        }

        // GET: Houses1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Houses1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HouseId,ownerId,title,Address,Description,ContactPhone,Province,District,Ward,Status,CreatedAt,UpdatedAt")] House house, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                _context.Add(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    // Save file to a storage location (e.g., wwwroot/images)
                    var filePath = Path.Combine("wwwroot/img", image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    var houseImage = new Image
                    {
                        HouseId = house.HouseId, // Use the saved house's ID
                        ImageUrl = $"/img/{image.FileName}"
                    };
                    _context.Add(houseImage);
                }
                await _context.SaveChangesAsync();
            }

           
            return View(house);
        }

        // GET: Houses1/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.House == null)
            {
                return NotFound();
            }

            var house = await _context.House.Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.HouseId == id); ;
            if (house == null)
            {
                return NotFound();
            }
           

            return View(house);
        }

       
        // POST: Houses1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HouseId,ownerId,title,Address,Description,ContactPhone,Province,District,Ward,Status,CreatedAt,UpdatedAt")] House house)
        {
            if (id != house.HouseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(house);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseExists(house.HouseId))
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

            return View(house);
        }

        // POST: Houses1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.House == null)
            {
                return Problem("Entity set 'RentHouseSystemContext.House'  is null.");
            }
            var house = await _context.House.FindAsync(id);
            if (house != null)
            {
                _context.House.Remove(house);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseExists(string id)
        {
            return (_context.House?.Any(e => e.HouseId == id)).GetValueOrDefault();
        }

        }
}


