using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentHouseSystem.Data;
using RentHouseSystem.Models;

namespace RentHouseSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly RentHouseSystemContext _context;
        public AdminController(RentHouseSystemContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
		public async Task<IActionResult> CommentsIndex(string id)
		{
			var comments = _context.Comment
				.Where(m => m.HouseId == id)
				.ToList();
			var house = await _context.House
				 .FirstOrDefaultAsync(m => m.HouseId == id);
			return View(comments); // Pass the comments to the view
		}
		public async Task<IActionResult> HouseIndex()
        {
            return _context.House != null ?
                        View(await _context.House.ToListAsync()) :
                        Problem("Entity set 'RentHouseSystemContext.House'  is null.");
        }
        public async Task<IActionResult> DetailHouse(string id)
        {
            if (id == null || _context.House == null)
            {
                return NotFound();
            }

            var house = await _context.House
                 .FirstOrDefaultAsync(m => m.HouseId == id);
            var facilities = await _context.Facilities
                 .FirstOrDefaultAsync(m => m.HouseId == id);
            var facilititesHouse = new FacilitiesHouse()
            {
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FacilitiesHouse facilititesHouse, List<IFormFile> images)
        {
            var house = new House
            {
                HouseId = facilititesHouse.HouseId,
                ownerId = facilititesHouse.ownerId,
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
        public async Task<IActionResult> CheckRentalInfor()
        {
            var pendingHouses = _context.House
        .Where(h => h.Status == status.pencding) 
                                              
        .ToList();

            return View(pendingHouses);          
                          
        }
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
