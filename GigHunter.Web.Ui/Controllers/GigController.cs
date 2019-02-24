using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GigHunter.Web.Ui.Controllers
{
    public class GigController : Controller
    {
        public IActionResult Gig()
        {
            return View();
        }
    }
}