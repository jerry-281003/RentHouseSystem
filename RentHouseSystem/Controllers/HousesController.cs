using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RentHouseSystem.Data;
using RentHouseSystem.Migrations;
using RentHouseSystem.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
        public async Task<IActionResult> Filter(List<string> id, bool ac, bool pets)
        {
            // Ensure the houseSearchModel list is empty before starting the process
            var houseSearchModel = new List<HouseSearchModel>();
            

            foreach (var item in id)
            {
                var facilities = await _context.Facilities
                    .FirstOrDefaultAsync(f => f.HouseId == item);

                if (facilities != null)
                {
                    var query = _context.House.Include(h => h.Images).Where(h => h.HouseId == item);

                    if (ac != true && pets != true)
                    {
                        query = query.Where(h => ac == facilities.AirConditioning && pets == facilities.Pet);
                    }
                    else if (ac != true && pets==false)
                    {
                        query = query.Where(h => ac == facilities.AirConditioning);
                    }
                    else if (pets != null && ac==false)
                    {
                        query = query.Where(h => pets == facilities.Pet);
                    }

                    var result = query.Select(h => new HouseSearchModel
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
                    });

                    houseSearchModel.AddRange(result);
                }

            
            }

            return View(houseSearchModel);
        }

        public IActionResult Search(string keyword, string district, string province)
        {
           
            if (district == null || province == null)
            {
                var result = _context.House
                            .Include(h => h.Images) // Include related images
                            .Where(t => EF.Functions.Like(t.title, $"%{keyword}%") && t.Status == status.approved && t.Invisible !=true||
                            EF.Functions.Like(t.Description, $"%{keyword}%") && t.Status == status.approved && t.Invisible != true)
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
            else 
            {
                var result = _context.House
                            .Include(h => h.Images) // Include related images
                            .Where(t => EF.Functions.Like(t.title, $"%{keyword}%") && t.Status == status.approved && t.Province==province && t.District==district && t.Invisible != true ||
                            EF.Functions.Like(t.Description, $"%{keyword}%") && t.Status == status.approved &&  t.Province == province && t.District == district && t.Invisible != true)
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
         
            return View(null);
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
        [Authorize(Roles = "owner")]
        // GET: Houses1/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.House == null)
            {
                return NotFound();
            }

            var house = await _context.House
                .Include(h => h.Images)
                .FirstOrDefaultAsync(m => m.HouseId == id);
            var facilities = await _context.Facilities
                 .FirstOrDefaultAsync(m => m.HouseId == id);

            var FacilitiesHouseViewModel = new FacilitiesHouseViewModel()
            {
               House = house,
               Facilities = facilities
            };

            if (house == null)
			{
				return NotFound();
			}

			return View(FacilitiesHouseViewModel);
		}

        [Authorize(Roles = "owner")]
        // POST: Houses1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("House,Facilities")] FacilitiesHouseViewModel facilitiesHouseViewModel, List<IFormFile> uploadedImages, List<string> imagesToDelete)
		{
            if (id != facilitiesHouseViewModel.House.HouseId)
            {
                return NotFound();
            }
            else
            {

                facilitiesHouseViewModel.House.UpdatedAt = DateTime.Now;

                if (imagesToDelete != null && imagesToDelete.Count > 0)
                {
                    foreach (var image in imagesToDelete)
                    {
                        var imageToRemove = await _context.Image.FindAsync(int.Parse(image));
                        if (imageToRemove != null)
                        {
                            // Optionally delete the physical file from storage
                            var imagePath = Path.Combine("wwwroot", imageToRemove.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                            _context.Image.Remove(imageToRemove);
                        }
                    }
                }
                // Handle Images
                if (uploadedImages != null)
                {
                    foreach (var imageFile in uploadedImages)
                    {
                        if (imageFile.Length > 0)
                        {
                            var image = new Image
                            {
                                ImageUrl = await UploadImageToStorage(imageFile) // Replace with a method to save image
                            };
                            facilitiesHouseViewModel.House.Images.Add(image);
                        }
                    }
                }

                _context.Update(facilitiesHouseViewModel.House);
                _context.Update(facilitiesHouseViewModel.Facilities);

                await _context.SaveChangesAsync();


                return RedirectToAction("HouseIndex", "Admin");
            }
			return View(facilitiesHouseViewModel);
		}
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> Delete(string? id)
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
			if (house != null)
			{
				house.Invisible = true;
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("HouseIndex", "Admin");
			
		}
		// POST: Houses1/Delete/5
		

        public bool HouseExists(string id)
        {
            return (_context.House?.Any(e => e.HouseId == id)).GetValueOrDefault();
        }
		public async Task<string> UploadImageToStorage(IFormFile file)
		{
			var fileName = Path.GetFileName(file.FileName);
			var filePath = Path.Combine("wwwroot/img", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return "/img/" + fileName; // Return relative URL
		}
	}
}


