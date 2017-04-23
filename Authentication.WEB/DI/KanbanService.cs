using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class KanbanService : IKanbanService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public List<kanbanboard> GetAllBoards()
        {
            return _db.kanbanboards.ToList();
        }

        public kanbanboard GetBoardById(int BoardId)
        {
            return _db.kanbanboards.Where(x => x.Id == BoardId).FirstOrDefault();
        }

        public void AddBoard(string Name, string Description, string Color)
        {
            _db.kanbanboards.Add(new kanbanboard
            {
                Name = Name,
                Description = Description,
                Color = Color
            });
            _db.SaveChanges();
        }

        public void UpdateBoardName(int BoardId, string Name)
        {
            kanbanboard board = GetBoardById(BoardId);
            board.Name = Name;
            _db.SaveChanges();
        }

        public void UpdateBoardDescription(int BoardId, string Description)
        {
            kanbanboard board = GetBoardById(BoardId);
            board.Description = Description;
            _db.SaveChanges();
        }

        public void UpdateBoardColor(int BoardId, string Color)
        {
            kanbanboard board = GetBoardById(BoardId);
            board.Color = Color;
            _db.SaveChanges();
        }

        public List<kanbanpoollist> GetAllPoolLists()
        {
            return _db.kanbanpoollists.ToList();
        }

        public List<kanbanpoollist> GetPoolListsForBoard(int BoardId)
        {
            return _db.kanbanpoollists.Where(x => x.KanbanBoardId == BoardId).ToList();
        }

        public kanbanpoollist GetPoolListById(int PoolListId)
        {
            return _db.kanbanpoollists.Where(x => x.Id == PoolListId).FirstOrDefault();
        }

        public void AddPoolList(string Name, string Description, int BoardId)
        {
            _db.kanbanpoollists.Add(new kanbanpoollist
            {
                Name = Name,
                Description = Description,
                KanbanBoardId = BoardId
            });
            _db.SaveChanges();
        }

        public void UpdatePoolListName(int PoolListId, string Name)
        {
            kanbanpoollist poolList = GetPoolListById(PoolListId);
            poolList.Name = Name;
            _db.SaveChanges();
        }

        public void UpdatePoolListDescription(int PoolListId, string Description)
        {
            kanbanpoollist poolList = GetPoolListById(PoolListId);
            poolList.Description = Description;
            _db.SaveChanges();
        }

        public void UpdatePoolListOrder(int PoolListId, float PrevOrder, float NextOrder)
        {
            kanbanpoollist poolList = GetPoolListById(PoolListId);
            poolList.OrderBy = (PrevOrder + NextOrder) / 2;
            _db.SaveChanges();
        }

        public List<kanbanticket> GetAllTickets()
        {
            return _db.kanbantickets.ToList();
        }

        public List<kanbanticket> GetTicketsForPoolList(int PoolListId)
        {
            return _db.kanbantickets.Where(x => x.KanbanPoolListId == PoolListId).ToList();
        }

        public List<kanbanticket> GetTicketsByCreator(int CreatebById)
        {
            return _db.kanbantickets.Where(x => x.CreatedBy == CreatebById).ToList();
        }

        public List<kanbanticket> GetTicketsByAssignedTo(int AssignedToId)
        {
            return _db.kanbantickets.Where(x => x.AssignedTo == AssignedToId).ToList();
        }

        public kanbanticket GetTicketById(int TicketId)
        {
            return _db.kanbantickets.Where(x => x.Id == TicketId).FirstOrDefault();
        }

        public void AddTicket(string Name, string Description, int CreatedById, int AssignedToId)
        {
            _db.kanbantickets.Add(new kanbanticket
            {
                Name = Name,
                Description = Description,
                CreatedBy = CreatedById,
                AssignedTo = AssignedToId
            });
            _db.SaveChanges();
        }

        public void UpdateTicketName(int TicketId, string Name)
        {
            kanbanticket ticket = GetTicketById(TicketId);
            ticket.Name = Name;
            _db.SaveChanges();
        }

        public void UpdateTicketDescription(int TicketId, string Description)
        {
            kanbanticket ticket = GetTicketById(TicketId);
            ticket.Description = Description;
            _db.SaveChanges();
        }

        public void UpdateTicketOrder(int TicketId, float PrevOrder, float NextOrder)
        {
            kanbanticket ticket = GetTicketById(TicketId);
            ticket.OrderBy = (PrevOrder + NextOrder) / 2;
            _db.SaveChanges();
        }

        public void UpdateTicketAssignedTo(int TicketId, int AssignedToId)
        {
            kanbanticket ticket = GetTicketById(TicketId);
            ticket.AssignedTo = AssignedToId;
            _db.SaveChanges();
        }

        public bool TicketNeedsReminder(int TicketId)
        {
            return true;
        }
    }
}