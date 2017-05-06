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
        private IPolicyTypeService _policyTypeService;

        public KanbanController(IKanbanService kanbanService, IUserService userService,
            IPolicyTypeService policyTypeService)
        {
            _kanbanService = kanbanService;
            _userService = userService;
            _policyTypeService = policyTypeService;
        }

        // GET: Kanban
        public ActionResult Index(int Mode = 0)
        {
            ViewBag.Mode = Mode;
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

        [HttpPost]
        public ActionResult AddTicket(FormCollection collection)
        {
            var ticket = _kanbanService.AddTicket(collection, _userService.GetUserIdByUsername(User.Identity.Name));
            return PartialView("_Ticket", ticket);
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

        public ActionResult GetUsersDropdown(string role = "")
        {
            ViewBag.Role = role;
            return PartialView("_UsersDropdown", _userService.GetAllUsers());
        }

        public ActionResult GetTicketTypesDropdown()
        {
            return PartialView("_TicketTypesDropdown", _kanbanService.GetAllTicketTypes());
        }

        public ActionResult GetPolicyTypesDropdown()
        {
            return PartialView("_PolicyTypesDropdown", _policyTypeService.GetAllPolicyType());
        }
    }
}