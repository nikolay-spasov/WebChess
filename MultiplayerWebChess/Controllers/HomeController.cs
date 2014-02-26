using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MultiplayerWebChess.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!WebSecurity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("GameLobby", "Game");
            }
        }
    }
}
