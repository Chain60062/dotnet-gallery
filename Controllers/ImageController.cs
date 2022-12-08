using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Gallery.Controllers
{
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = null!;
        private readonly ApplicationDbContext _db = null!;
        private readonly UserManager<IdentityUser> _userManager = null!;
        public ImageController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Authorize]
        [Route("Gallery")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
            var images = await _db.Images.Where(Image => Image.UserId == userId).ToListAsync();
            return View(images);
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<JsonResult> GetOne(int id)
        {
            var image = await _db.Images.FirstOrDefaultAsync(i => i.ImageId == id);
            return Json(image);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ImageViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            else
            {
                var image = new Image();
                image.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string wwwroot = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.PictureFile.FileName);
                string extension = Path.GetExtension(model.PictureFile.FileName);
                image.Picture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                image.AddedAt = DateTime.Now;

                if (ModelState.IsValid)
                {
                    string path = Path.Combine(wwwroot + "/images/", fileName);
                    using (FileStream fileStream = new(path, FileMode.Create))
                    {
                        await model.PictureFile.CopyToAsync(fileStream);
                    }
                    await _db.Images.AddAsync(image);

                    await _db.SaveChangesAsync();

                    return Redirect("/Gallery");
                }
                return Redirect(nameof(Index));
            }
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _db.Images.FirstOrDefaultAsync(i => i.ImageId == id);

            _db.Images.Remove(image);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Image");
        }
        [HttpGet("[controller]/[action]/{albumId}")]
        public async Task<IActionResult> SelectImage(int albumId)
        {
            var userId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
            var albumImages = await _db.Albums.Where(a => a.AlbumId == albumId).SelectMany(a => a.Images).ToListAsync();
            var images = await _db.Images.Where(Image => Image.UserId == userId).ToListAsync();
            var result = images.Except(albumImages).ToList();
            ViewData["albumId"] = albumId;
            return View(result);
        }
    }
}