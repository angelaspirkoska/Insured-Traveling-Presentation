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

        public kanbanpoollist AddPoolList(string Name, string Description, int BoardId)
        {
            var highestOrder = _db.kanbanpoollists.Max(x => x.OrderBy);

            var newPoolList = new kanbanpoollist
            {
                Name = Name,
                Description = Description,
                KanbanBoardId = BoardId,
                OrderBy = highestOrder + 1
            };
            _db.kanbanpoollists.Add(newPoolList);
            _db.SaveChanges();
            return newPoolList;
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

        public void UpdatePoolListOrder(List<int> order)
        {
            for (int i = 1; i <= order.Count; i++)
            {
                var poolId = order[i - 1];
                var pool = _db.kanbanpoollists.FirstOrDefault(x => x.Id == poolId);
                if (pool != null)
                {
                    pool.OrderBy = i;
                }
            }
            _db.SaveChanges();
        }

        public void RemovePoolList(int poolListId)
        {
            kanbanpoollist poolListForRemove = _db.kanbanpoollists.Where(x => x.Id == poolListId).FirstOrDefault();
            _db.kanbanpoollists.Remove(poolListForRemove);
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

        public List<kanbanticket> GetTicketsByAssignedTo(string AssignedToId = null)
        {
            List<kanbanticket> tickets = new List<kanbanticket>();
            foreach (var timekeeper in _db.kanbantimekeepers)
            {
                if (timekeeper.AssignedToId == AssignedToId || AssignedToId == null)
                    tickets.Add(timekeeper.kanbanticket);
            }
            return tickets;
        }

        public kanbanticket GetTicketById(int TicketId)
        {
            return _db.kanbantickets.Where(x => x.Id == TicketId).FirstOrDefault();
        }

        public List<kanbantickettypecomponent> GetComponentsForTicketType(int ticketTypeId)
        {
            kanbantickettype ticketType = _db.kanbantickettypes.Where(x => x.ID == ticketTypeId).FirstOrDefault();
            return ticketType.kanbantickettypecomponents.ToList();
        }

        public kanbanticket AddTicket(string Name, string Description, int CreatedById, int AssignedToId, int PoolListId, List<string> users)
        {
            float highestOrder = 0;
            if (_db.kanbantickets.Count() > 0)
                highestOrder = _db.kanbantickets.Max(x => x.OrderBy);
            var ticket = new kanbanticket
            {
                Name = Name,
                Description = Description,
                OrderBy = highestOrder + 1,
                CreatedBy = CreatedById,
                AssignedTo = AssignedToId,
                KanbanPoolListId = PoolListId
            };
            _db.kanbantickets.Add(ticket);
            _db.SaveChanges();

            foreach (var user in users)
            {
                _db.kanbantimekeepers.Add(new kanbantimekeeper
                {
                    AssignedToId = user,
                    KanbanTicketId = ticket.Id,
                    AssignedDateTime = DateTime.Now
                });
            }
            _db.SaveChanges();
            return ticket;
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

        public void UpdateTicketOrder(List<int> order)
        {
            for (int i = 1; i <= order.Count; i++)
            {
                var ticketId = order[i - 1];
                var ticket = _db.kanbantickets.FirstOrDefault(x => x.Id == ticketId);
                if (ticket != null)
                {
                    ticket.OrderBy = i;
                }
            }
            _db.SaveChanges();
        }

        public void UpdateTicketAssignedTo(int TicketId, int AssignedToId)
        {
            
        }

        public bool TicketNeedsReminder(int TicketId)
        {
            return true;
        }

        public void ChangeTicketPool(int ticketId, int newPoolId, List<int> order)
        {
            var ticket = _db.kanbantickets.FirstOrDefault(x => x.Id == ticketId);
            if (ticket != null)
            {
                ticket.KanbanPoolListId = newPoolId;
            }
            _db.SaveChanges();
            UpdateTicketOrder(order);
        }

        public void RemoveTicket(int ticketId)
        {
            kanbanticket ticketForRemove = _db.kanbantickets.Where(x => x.Id == ticketId).FirstOrDefault();
            _db.kanbantickets.Remove(ticketForRemove);
            _db.SaveChanges();
        }

        public List<kanbantickettype> GetAllTicketTypes()
        {
            return _db.kanbantickettypes.ToList();
        }
    }
}