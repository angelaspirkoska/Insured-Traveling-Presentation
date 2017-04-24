using InsuredTraveling.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class KanbanController : Controller
    {
        private IKanbanService _kanbanService;

        public KanbanController(IKanbanService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        // GET: Kanban
        public ActionResult Index()
        {
            return View(_kanbanService.GetAllBoards().FirstOrDefault());
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}