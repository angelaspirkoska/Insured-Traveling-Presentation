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

        public List<kanbanpoollist> GetAllPoolLists()
        {
            return _db.kanbanpoollists.ToList();
        }

        public List<kanbanticket> GetAllTickets()
        {
            return _db.kanbantickets.ToList();
        }
    }
}