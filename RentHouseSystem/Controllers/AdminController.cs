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
                house.Status = status.approved;
                _context.Update(house);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Admin");
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
			return RedirectToAction("Index", "Admin");
        }
    }
}
