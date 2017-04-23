using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    interface IKanbanService
    {
        List<kanbanboard> GetAllBoards();

        List<kanbanpoollist> GetAllPoolLists();

        List<kanbanticket> GetAllTickets();
    }
}
