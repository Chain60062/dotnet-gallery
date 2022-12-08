using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Gallery.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Gallery.Models;

namespace Gallery.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ApplicationDbContext _db = null!;
        private readonly IWebHostEnvironment _webHostEnvironment = null!;
        private readonly UserManager<IdentityUser> _userManager;
        public AlbumController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

                // var albumImages = await _db.Albums.Where(a => a.AlbumId == id).SelectMany(a => a.Images).ToListAsync();
                var albums = await _db.Albums.Where(a => a.UserId == userId).Include(a => a.Images).ToListAsync();

                return View(albums);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AlbumViewModel model)
        {
            Album album = new();

            if (ModelState.IsValid)
            {
                album.AlbumName = model.AlbumName;
                album.Description = model.Description;
                album.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _db.Albums.AddAsync(album);

                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Album");
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            Album albumImages = new();
            albumImages.Images = await _db.Albums.Where(a => a.AlbumId == id).SelectMany(a => a.Images).ToListAsync();
            albumImages.AlbumId = id;
            return View(albumImages);
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album is null)
            {
                return NotFound();
            }
            _db.Albums.Remove(album);

            await _db.SaveChangesAsync();

            return Redirect("/Album");
        }
        [HttpPost("[controller]/[action]/{albumId}/{imageId}")]
        public async Task<IActionResult> AddImage(int albumId, int imageId)
        {
            var album = await _db.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId);
            var image = await _db.Images.FirstOrDefaultAsync(i => i.ImageId == imageId);
            if (album is null || image is null)
            {
                return NotFound();
            }
            image.Albums?.Add(album);
        
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Album");
        }

    }
}
// var albumImages = await _db.Albums.Where(a => a.AlbumId == id).SelectMany(a => a.Images).ToListAsync();
// public async Task CreateAsync(Album a)
// {
//     EntityEntry<Album> added = await _db.Albums.AddAsync(a);

//     await _db.SaveChangesAsync();
// }

// public async Task DeleteAsync(int id)
// {
//     var album = await _db.Albums.FirstOrDefaultAsync(a => a.AlbumId == id);

//     _db.Albums.Remove(album);

//     await _db.SaveChangesAsync();
// }

// public async Task<IActionResult> AddImage(int id)
// {
//     var album = await _db.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId);
//     var image = await _db.Images.FirstOrDefaultAsync(i => i.ImageId == imageId);
//     image.Albums.Add(album);

//     await _db.SaveChangesAsync();
//     return View();
// }