using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentHouseSystem.Data;
using RentHouseSystem.Models;


namespace RentHouseSystem
{
    public class CommentsController : Controller
    {
        private readonly RentHouseSystemContext _context;
        

        public CommentsController(RentHouseSystemContext context)
        {
            _context = context;
           
        }
        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com") // Replace with your SMTP details
            {
                Port = 587,
                Credentials = new NetworkCredential("thanhdn21@uef.edu.vn", "thanhpro123"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("thanhdn21@uef.edu.vn"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        [HttpPost]
        public async Task<IActionResult> Repline(int commnetId, string commentContent)
        {
            
            var comment = await _context.Comment.FirstOrDefaultAsync(m => m.CommentId == commnetId);
            var reply = new Comment
            {
                
                HouseId = comment.HouseId, // Optionally link to the same house
                UserId = comment.UserId, // Current logged-in user
                CommentContent = commentContent,
                email = comment.email, // Ensure the email is fetched
                ReplineId = commnetId, // Link the reply to the parent comment
                CreatedAt = DateTime.Now
            };
            _context.Add(reply);
            _context.SaveChanges();
            return RedirectToAction("CommentsIndex", "Admin", new { id = comment.HouseId });
        }
        
        public async Task<IActionResult> UpComment(string HouseId, string CommentContent)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("You need to log in to comment.");
            }

            var house = await _context.House.FirstOrDefaultAsync(m => m.HouseId == HouseId);
            
            // Find the UserId based on User.Identity.Name
            var user = _context.Users.FirstOrDefault(u => u.Id == house.ownerId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create the comment object
            var comment = new Comment
            {
                UserId = user.Id,
                HouseId = HouseId,
                email= user.Email,
                CommentContent = CommentContent,
                CreatedAt = DateTime.Now,
                ReplineId =null
            };
            var subject = "Notify comment about "+house.title;
            SendEmailAsync(comment.email,subject, comment.CommentContent);
            // Add to the database and save changes
            _context.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("DetailHouse", "Admin", new { id = HouseId });
        }
        // GET: Comments
        public async Task<IActionResult> Index()
        {
              return _context.Comment != null ? 
                          View(await _context.Comment.ToListAsync()) :
                          Problem("Entity set 'RentHouseSystemContext.Comment'  is null.");
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,HouseId,UserId,CommentContent,email,CreatedAt,ReplineId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,HouseId,UserId,CommentContent,email,CreatedAt")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("DetailHouse","Admin");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'RentHouseSystemContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comment?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
