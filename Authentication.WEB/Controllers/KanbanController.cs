using InsuredTraveling.DI;
using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        private IUserService _userService;

        public KanbanController(IKanbanService kanbanService, IUserService userService)
        {
            _kanbanService = kanbanService;
            _userService = userService;
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

        public ActionResult AddTicketPartial(int ticketTypeId)
        {
            List<kanbantickettypecomponent> list = _kanbanService.GetComponentsForTicketType(ticketTypeId);
            return PartialView("_AddTicket", list);
        }

        public ActionResult AddTicket(int poolListId, string title, string description, List<string> users)
        {
            return PartialView("_Ticket", _kanbanService.AddTicket(title, description, 1, 1, poolListId, users));
        }

        public void ChangeTicketPool(int ticketId, int newPoolId, List<int> order)
        {
            _kanbanService.ChangeTicketPool(ticketId, newPoolId, order);
        }

        public ActionResult TicketDetails(int ticketId)
        {
            kanbanticket ticket = _kanbanService.GetTicketById(ticketId);
            return PartialView("_TicketDetails", ticket);
        }

        public void DeleteTicket(int ticketId)
        {
            _kanbanService.RemoveTicket(ticketId);
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

        public void DeletePoolList(int poolListId)
        {
            _kanbanService.RemovePoolList(poolListId);
        }

        public void ChangePoolsOrder(List<int> order)
        {
            _kanbanService.UpdatePoolListOrder(order);
        }

        public ActionResult GetUsersDropdown()
        {
            return PartialView("_UsersDropdown", _userService.GetAllUsers());
        }

        public ActionResult GetTicketTypesDropdown()
        {
            return PartialView("_TicketTypesDropdown", _kanbanService.GetAllTicketTypes());
        }
    }
}