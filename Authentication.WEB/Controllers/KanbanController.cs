using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Models;
using InsuredTraveling.SignalR.Hubs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [SessionExpire]
    public class KanbanController : Controller
    {
        private IKanbanService _kanbanService;
        private IUserService _userService;
        private IPolicyTypeService _policyTypeService;
        private INotificationService _notificationService;

        public KanbanController(IKanbanService kanbanService, IUserService userService,
            IPolicyTypeService policyTypeService, INotificationService notificationService)
        {
            _kanbanService = kanbanService;
            _userService = userService;
            _policyTypeService = policyTypeService;
            _notificationService = notificationService;
        }

        // GET: Kanban
        public ActionResult Index(int Mode = 0)
        {
            ViewBag.Mode = Mode;
            return View(_kanbanService.GetAllBoards().FirstOrDefault());
        }

        public ActionResult Admin()
        {
            return View(_kanbanService.GetAllTicketTypes());
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult AddTicketPartial(int ticketTypeId, int editing)
        {
            List<kanbantickettypecomponent> list = _kanbanService.GetComponentsForTicketType(ticketTypeId);
            ViewBag.Editing = editing;
            return PartialView("_AddTicket", list);
        }

        [HttpPost]
        public ActionResult AddTicket(FormCollection collection)
        {
            var ticket = _kanbanService.AddTicket(collection, _userService.GetUserIdByUsername(User.Identity.Name));

            //notifications for assignees
            var assigneesNotification = _notificationService.AddNotification("New ticket created!", "You've been assigned to newly created ticket: " + ticket.Name + ".");
            if (collection["assignees"] != null)
                foreach (var userId in collection["assignees"].Split(',').ToList())
                {
                    _notificationService.AddUserNotification(assigneesNotification.Id, userId);
                }

            //notifications for watchers
            var watchersNotification = _notificationService.AddNotification("New ticket created!", "You've been assigned to newly created ticket: " + ticket.Name + ", as watcher!");
            if (collection["watchers"] != null)
                foreach (var userId in collection["watchers"].Split(',').ToList())
                {
                    _notificationService.AddUserNotification(watchersNotification.Id, userId);
                }

            ViewBag.AssigneesNotification = assigneesNotification.Id;
            ViewBag.WatchersNotification = watchersNotification.Id;

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

        public ActionResult TicketEdit(int ticketId)
        {
            kanbanticket ticket = _kanbanService.GetTicketById(ticketId);
            return PartialView("_EditTicket", ticket);
        }

        public void DeleteTicket(int ticketId)
        {
            _kanbanService.RemoveTicket(ticketId);
        }

        public void DeleteTicketTypeComponent(int ticketTypeComponentId)
        {
            _kanbanService.RemoveTicketTypeComponent(ticketTypeComponentId);
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

        public ActionResult AddTicketTypeComponent(int ticketTypeId, int componentType, string componentName)
        {
            var component = _kanbanService.AddTicketTypeComponent(
                ticketTypeId, componentType, componentName);
            return PartialView("_TicketTypeComponent", component);
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

        public ActionResult GetComponentsDropdown()
        {
            return PartialView("_ComponentsDropdown", _kanbanService.GetAllComponents());
        }
    }
}