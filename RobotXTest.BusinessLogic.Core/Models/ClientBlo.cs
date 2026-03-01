using RobotXTest.DataAccess.Core.Models;

namespace RobotXTest.BusinessLogic.Core.Models
{
    public class ClientBlo
    {
        public List<ClientRto> Data { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
