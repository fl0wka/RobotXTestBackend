using System.Net;
using Microsoft.AspNetCore.Mvc;
using RobotXTest.BusinessLogic.Core.Interface;
using RobotXTest.BusinessLogic.Core.Models;
using RobotXTest.Core.Models;
using RobotXTest.DataAccess.Core.Models;

namespace RobotXTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        /// <summary>
        /// Получить список клиентов
        /// </summary>
        /// <param name="page">номер страницы</param>
        /// <param name="pageSize">размер страницы</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ClientBlo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            return Ok(await clientService.GetAll(page, pageSize));
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="clientDto"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ClientRto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("{cardCode}")]
        public async Task<IActionResult> Update(long cardCode, [FromBody] UpdateRequestDto clientDto)
        {
            return Ok(await clientService.Update(cardCode, clientDto));
        }

        /// <summary>
        /// Загрузить Excel файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ImportResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("Import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            using var stream = file.OpenReadStream();
            var result = await clientService.Import(stream);

            return Ok(new ImportResultDto
            {
                Message = $"Добавлено: {result.Added}, обновлено: {result.Updated}",
                Errors = result.Errors.Take(20).ToList()
            });
        }
    }
}
