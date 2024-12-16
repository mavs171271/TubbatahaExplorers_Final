using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VisualStudio.Models.ViewModels;
using VisualStudio.Areas.Identity.Data;
using VisualStudio.Models;
using Microsoft.AspNetCore.Identity;

namespace VisualStudio.Controllers
{
    [Authorize] // Require login for all actions in this controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IConfiguration configuration, ApplicationDbContext context, ILogger<HomeController> logger,UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        [AllowAnonymous] // Allow access without login for Index page
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            var speciesData = new
            {
                labels = new[] { "Algae", "Bony Fishes", "Cetaceans", "Corals", "Elasmobranch","Gastropods","Seagrass" },
                values = new[] { 65, 744, 13, 360, 29, 76, 8 }
            };
            return View(speciesData);
        }
        [AllowAnonymous] // Allow access without login for Privacy page
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Species()
        {
            Models.Content obj;
            List<Models.Content> lobj = new List<Content>();

            obj = new Content();
            obj.title = "Algae";
            obj.text = "Algae are simple nonflowering plantlike organisms ranging in size from single-celled diatoms (microalgae) to giant multicellular forms such as kelp or seaweed (macroalgae)";
            obj.img = "https://www.naturalworldheritagesites.org/wp-content/uploads/2021/03/Tubbataha-Reefs-Natural-Park.jpg";
            obj.link = "/Home/Algae";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Bony Fishes";
            obj.text = "Bony fishes are a diverse superclass of vertebrate animals that have endoskeletons primarily composed of bone tissue.";
            obj.img = "https://nplimages.infradoxxs.com/cache/pcache2/01586433.jpg";
            obj.link = "/Home/BonyFishes";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Cetaceans";
            obj.text = "Cetaceans are aquatic mammals belonging to the order Artiodactyla that includes whales, dolphins, and porpoises.";
            obj.img = "https://dtmag.com/wp-content/uploads/2018/08/dolphin-snyderman-1050x700.jpg";
            obj.link = "/Home/Cetaceans";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Corals";
            obj.text = "Corals are colonial marine invertebrates within the subphylum Anthozoa of the phylum Cnidaria. They typically form compact colonies of many identical individual polyps";
            obj.img = "https://miro.medium.com/v2/resize:fit:1400/1*rdcV3s3eLARn_o5i2U6hhg.jpeg";
            obj.link = "/Home/Corals";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Elasmobranch";
            obj.text = " Elasmobranchii is a subclass of Chondrichthyes or cartilaginous fish, including modern sharks, rays, skates, and sawfish.";
            obj.img = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Great_White_Shark_%2814730719119%29.jpg";
            obj.link = "/Home/Elasmobranch";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Invertebrates/Gastropods";
            obj.text = "card-text\">Gastropods, commonly known as slugs and snails, belong to a large taxonomic class of invertebrates within the phylum Mollusca called Gastropoda.";
            obj.img = "https://www.thoughtco.com/thmb/NC3nhHxa2nbE3hqlx0sVa4V-hqc=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/Conch-Bahamas-Reinhard-Dirscherl-WaterFrame-Getty-56a5f7fd3df78cf7728abf98.jpg";
            obj.link = "/Home/Gastropods";

            lobj.Add(obj);

            obj = new Content();
            obj.title = "Seagrass";
            obj.text = "Seagrasses are the only flowering plants that grow in marine environments. There are about 60 species of fully marine seagrasses which belong to four families, all in the order Alismatales.";
            obj.img = "https://images.prismic.io/ocean-agency-cms/eff7160a-e2ec-4eae-8e4e-0ab237e7d480_2000+seagrass+category.jpg?auto=compress,format";
            obj.link = "/Home/Seagrass";

            lobj.Add(obj);

            return View(lobj);
        }
        [AllowAnonymous]
        public IActionResult Algae()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult BonyFishes()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Cetaceans()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Corals()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Elasmobranch()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Gastropods()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Seagrass()
        {
            return View();
        }

    

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Articles(UploadedFileViewModel vm, int page = 1, int pageSize = 5)
        {
            var files = _context.UploadedFiles.ToList(); // Replace with your database fetch logic

            var paginatedFiles = files.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var viewModel = new UploadedFileViewModel
            {
                SystemFiles = paginatedFiles,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(files.Count / (double)pageSize)
            };
            return View(viewModel);

        
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(UploadedFileViewModel vm, IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + file.FileName;
            var path = _configuration.GetSection("FileManagement:SystemFileUploads").Value;
            var filepath = Path.Combine(path, filename);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var pendingFile = new PendingFile
            {
                UploadedAt = DateTime.Now,
                FileName = filename,
                Description = vm.Description,
                FilePath = filepath,
                Title = vm.Title,
                UserId = userId
            };
            await _context.PendingFiles.AddAsync(pendingFile);
            await _context.SaveChangesAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user != null && !user.TwoFactorEnabled)
            {
                return RedirectToPage("/Account/AddAuth", new { area = "Identity" });
            }
            vm.SystemFiles = await _context.UploadedFiles.ToListAsync();

            TempData["Message"] = "File uploaded successfully! Please wait for admin approval.";
            TempData["MessageType"] = "success";

            return RedirectToAction("Articles");
        }

        [AllowAnonymous]
        public IActionResult ViewPDF(int id)
        {
            var fileuploaded = _context.UploadedFiles.FirstOrDefault(f => f.Id == id);
            if (fileuploaded == null || string.IsNullOrEmpty(fileuploaded.FilePath))
            {
                return NotFound();
            }

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileuploaded.FilePath);
            byte[] filebytes = System.IO.File.ReadAllBytes(filepath);
            return File(filebytes, "application/pdf");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.UploadedFiles.FindAsync(id);
            if (file == null) return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.UploadedFiles.Remove(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Articles");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
