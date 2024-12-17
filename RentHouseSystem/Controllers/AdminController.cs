using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentHouseSystem.Data;
using RentHouseSystem.Models;
using System.Data;

namespace RentHouseSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly RentHouseSystemContext _context;
        public AdminController(RentHouseSystemContext context)
        {
            _context = context;

        }
        [Authorize(Roles = "Admin,owner")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CommentsIndex(string id)
		{
            var comments = await _context.Comment
                .Where(c => c.HouseId == id)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return View(comments);
        }
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> HouseIndex()
        {
            var house = await _context.House.Where(h => h.Invisible != true).ToListAsync();
            return View(house);
        }
        public async Task<IActionResult> DetailHouse(string id)
        {
            var house = await _context.House
     
     .FirstOrDefaultAsync(m => m.HouseId == id);

            if (house == null)
            {
                return NotFound("House not found.");
            }

            var facilities = await _context.Facilities
                .FirstOrDefaultAsync(m => m.HouseId == id);

            if (facilities == null)
            {
                return NotFound("Facilities not found.");
            }
            
            var facilititesHouse = new FacilitiesHouse()
            {
                Id= facilities.Id,
                HouseId = house.HouseId,
                ownerId = house.ownerId,
                title = house.title,
                Address = house.Address,
                Description = house.Description,
                ContactPhone = house.ContactPhone,
                Province = house.Province,
                District = house.District,
                Ward = house.Ward,
                Status = house.Status,
                CreatedAt = house.CreatedAt,
                UpdatedAt = house.UpdatedAt,
                CloseTime = house.CloseTime,
                AcceptableVehicles = house.AcceptableVehicles,
               Image= _context.Image.Where(h =>h.HouseId == id).ToList(),
                
                InventoryEquipment = facilities.InventoryEquipment,
                UtilityCost = facilities.UtilityCost,
                AirConditioning = facilities.AirConditioning,
                MajorAppliances = facilities.MajorAppliances,
                NumberOfBedroom = facilities.NumberOfBedroom,
                ToiletAndBathroom = facilities.ToiletAndBathroom,
                Pet = facilities.Pet,
                DepositRequired = facilities.DepositRequired
            };
            
            return View(facilititesHouse);
        }
        [Authorize(Roles = "owner")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "owner")]
        [HttpPost]
        public async Task<IActionResult> Create(FacilitiesHouse facilititesHouse, List<IFormFile> images)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            var house = new House
            {
                HouseId = facilititesHouse.HouseId,
                ownerId = user.Id,
                title = facilititesHouse.title,
                Address = facilititesHouse.Address,
                Description = facilititesHouse.Description,
                ContactPhone = facilititesHouse.ContactPhone,
                Province = facilititesHouse.Province,
                District = facilititesHouse.District,
                Ward = facilititesHouse.Ward,
                Status = facilititesHouse.Status,
                CreatedAt = facilititesHouse.CreatedAt,
                UpdatedAt = facilititesHouse.UpdatedAt,
                CloseTime = facilititesHouse.CloseTime,
                AcceptableVehicles = facilititesHouse.AcceptableVehicles
            };
            var facilities = new Facilities
            {
                HouseId = facilititesHouse.HouseId,
                InventoryEquipment = facilititesHouse.InventoryEquipment,
                UtilityCost = facilititesHouse.UtilityCost,
                AirConditioning = facilititesHouse.AirConditioning,
                MajorAppliances = facilititesHouse.MajorAppliances,
                NumberOfBedroom = facilititesHouse.NumberOfBedroom,
                ToiletAndBathroom = facilititesHouse.ToiletAndBathroom,
                Pet = facilititesHouse.Pet,
                DepositRequired = facilititesHouse.DepositRequired
            };
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
            _context.Add(house);
            _context.Add(facilities);
            await _context.SaveChangesAsync();
            return RedirectToAction("HouseIndex", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckRentalInfor()
        {
            var pendingHouses = _context.House
        .Where(h => h.Status == status.pencding) 
                                              
        .ToList();

            return View(pendingHouses);          
                          
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approved(string id)
        {
            var house = await _context.House
                .FirstOrDefaultAsync(m => m.HouseId == id);
            if (house == null)
            {
				return NotFound();
			}
			house.Status = status.approved;
			_context.Update(house);
			await _context.SaveChangesAsync();
			return RedirectToAction("CheckRentalInfor", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Rejected(string id)
        {
            var house = await _context.House
                .FirstOrDefaultAsync(m => m.HouseId == id);
			if (house == null)
			{
				return NotFound(); 
			}
			house.Status = status.rejected;
			_context.Update(house);
			await _context.SaveChangesAsync();
			return RedirectToAction("CheckRentalInfor", "Admin");
        }
    }
}
