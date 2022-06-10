using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RandomPasscodeGen.Models;

namespace RandomPasscodeGen.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if(HttpContext.Session.GetInt32("CodeNumber") == null){
            HttpContext.Session.SetInt32("CodeNumber", 1);
        }
        if(HttpContext.Session.GetString("GeneratedCode") == null){
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            char chr;
            for (int i = 0; i < 14; i++)
            {
                int which = rand.Next(0, 2);
                if (which == 0)
                {
                    chr = Convert.ToChar(Convert.ToInt32(Math.Floor(26*rand.NextDouble() + 65)));
                    builder.Append(chr);
                }
                else
                {
                    int randNum = rand.Next(0, 10);
                    builder.Append(randNum);
                }
            }
            string GeneratedCode = builder.ToString();
            HttpContext.Session.SetString("GeneratedCode", GeneratedCode);
        }
        ViewBag.GeneratedCode = HttpContext.Session.GetString("GeneratedCode");
        ViewBag.CodeNumber = HttpContext.Session.GetString("CodeNumber");
        return View();
    }
    
    [HttpPost("")]
        public IActionResult GeneratePasscode()
        {
            int? Passcode = HttpContext.Session.GetInt32("codeNumber");
            int NewPasscode = Passcode ?? default(int);
            HttpContext.Session.SetInt32("codeNumber", NewPasscode + 1); 

            // Generate a new passcode and store in session
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 14; i++)
            {
                int option = rand.Next(0, 2);
                if (option == 0)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                    builder.Append(ch);
                }
                else
                {
                    int randNum = rand.Next(0, 10);
                    builder.Append(randNum);
                }
            }
            string GeneratedPasscode = builder.ToString();
            HttpContext.Session.SetString("GeneratedPasscode", GeneratedPasscode);
            
            // redirect to Index
            return RedirectToAction("Index");
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
