using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public interface IKanbanService
    {
        List<kanbanboard> GetAllBoards();

        kanbanboard GetBoardById(int BoardId);

        void AddBoard(string Name, string Description, string Color);

        void UpdateBoardName(int BoardId, string Name);

        void UpdateBoardDescription(int BoardId, string Description);

        void UpdateBoardColor(int BoardId, string Color);

        List<kanbanpoollist> GetAllPoolLists();

        List<kanbanpoollist> GetPoolListsForBoard(int BoardId);

        kanbanpoollist GetPoolListById(int PoolListId);

        kanbanpoollist AddPoolList(string Name, string Description, int BoardId);

        void ChangeTicketPool(int ticketId, int newPoolId, List<int> order);

        void UpdatePoolListName(int PoolListId, string Name);

        void UpdatePoolListDescription(int PoolListId, string Description);

        void UpdatePoolListOrder(List<int> order);

        void RemovePoolList(int poolListId);

        List<kanbanticket> GetAllTickets();

        List<kanbanticket> GetTicketsForPoolList(int PoolListId);

        List<kanbanticket> GetTicketsByCreator(string createbById);

        List<kanbanticket> GetTicketsByAssignedTo(string AssignedToId);

        kanbanticket GetTicketById(int TicketId);

        kanbanticket AddTicket(FormCollection collection, string createdBy);

        void UpdateTicketName(int TicketId, string Name);

        void UpdateTicketDescription(int TicketId, string Description);

        void UpdateTicketOrder(List<int> order);

        void UpdateTicketAssignedTo(int TicketId, int AssignedToId);

        bool TicketNeedsReminder(int TicketId);

        void RemoveTicket(int ticketId);

        List<kanbantickettypecomponent> GetComponentsForTicketType(int ticketTypeId);

        List<kanbantickettype> GetAllTicketTypes();

        List<kanbancomponent> GetAllComponents();
        kanbantickettypecomponent AddTicketTypeComponent(int ticketTypeId, int componentType, string componentName);
    }
}
