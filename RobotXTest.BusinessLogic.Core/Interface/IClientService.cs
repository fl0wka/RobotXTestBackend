using RobotXTest.BusinessLogic.Core.Models;
using RobotXTest.Core.Models;
using RobotXTest.DataAccess.Core.Models;

namespace RobotXTest.BusinessLogic.Core.Interface
{
    public interface IClientService
    {
        Task<ClientBlo> GetAll(int page = 1, int pageSize = 50);
        Task<ClientRto> Update(long cardCode, UpdateRequestDto clientDto);
        Task<ImportResultBlo> Import(Stream fileStream);
    }
}
