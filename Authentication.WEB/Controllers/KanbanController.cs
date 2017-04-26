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

        public void ChangeTicketPool(int ticketId, int newPoolId, List<int> order)
        {
            _kanbanService.ChangeTicketPool(ticketId, newPoolId, order);
        }

        public void ChangeTicketOrder(List<int> order)
        {
            _kanbanService.UpdateTicketOrder(order);
        }

        public ActionResult AddPoolList(int boardId, string name)
        {
            ViewBag.IsAddingPool = true;
            return PartialView("_PoolList", _kanbanService.AddPoolList(name, "", boardId));
        }

        public void ChangePoolsOrder(List<int> order)
        {
            _kanbanService.UpdatePoolListOrder(order);
        }
    }
}