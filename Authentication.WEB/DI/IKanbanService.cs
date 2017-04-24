using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        void AddPoolList(string Name, string Description, int BoardId);

        void UpdatePoolListName(int PoolListId, string Name);

        void UpdatePoolListDescription(int PoolListId, string Description);

        void UpdatePoolListOrder(int PoolListId, float PrevOrder, float NextOrder);

        List<kanbanticket> GetAllTickets();

        List<kanbanticket> GetTicketsForPoolList(int PoolListId);

        List<kanbanticket> GetTicketsByCreator(int CreatebById);

        List<kanbanticket> GetTicketsByAssignedTo(int AssignedToId);

        kanbanticket GetTicketById(int TicketId);

        void AddTicket(string Name, string Description, int CreatedById, int AssignedToId);

        void UpdateTicketName(int TicketId, string Name);

        void UpdateTicketDescription(int TicketId, string Description);

        void UpdateTicketOrder(int TicketId, float PrevOrder, float NextOrder);

        void UpdateTicketAssignedTo(int TicketId, int AssignedToId);

        bool TicketNeedsReminder(int TicketId);
    }
}
