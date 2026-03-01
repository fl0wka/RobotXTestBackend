using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using RobotXTest.BusinessLogic.Core.Interface;
using RobotXTest.BusinessLogic.Core.Models;
using RobotXTest.Core.Models;
using RobotXTest.DataAccess.Core.Models;
using RobotXTest.DataAccess.DbContexts;
using RobotXTest.Shared.Helpers;

namespace RobotXTest.BusinessLogic.Services
{
    public class ClientService : IClientService
    {
        private readonly RobotXTestContext context;

        public ClientService(RobotXTestContext context)
        {
            this.context = context;
        }

        public async Task<ClientBlo> GetAll(int page = 1, int pageSize = 50)
        {
            var totalCount = await context.Clients.CountAsync();
            var clients = await context.Clients
                .OrderBy(c => c.CardCode)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new ClientBlo
            {
                Data = clients,
                Total = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return response;
        }

        public async Task<ClientRto> Update(long cardCode, UpdateRequestDto clientDto)
        {
            var clientRto = await context.Clients.FirstOrDefaultAsync(c => c.CardCode == cardCode)
                ?? throw new Exception("Client not found");

            clientRto.LastName = clientDto.LastName ?? clientRto.LastName;
            clientRto.FirstName = clientDto.FirstName ?? clientRto.FirstName;
            clientRto.SurName = clientDto.SurName ?? clientRto.SurName;
            clientRto.PhoneMobile = clientDto.PhoneMobile ?? clientRto.PhoneMobile;
            clientRto.Email = clientDto.Email ?? clientRto.Email;
            clientRto.GenderId = clientDto.GenderId ?? clientRto.GenderId;
            clientRto.Birthday = clientDto.Birthday ?? clientRto.Birthday;
            clientRto.City = clientDto.City ?? clientRto.City;
            clientRto.Pincode = clientDto.Pincode ?? clientRto.Pincode;
            clientRto.Bonus = clientDto.Bonus ?? clientRto.Bonus;
            clientRto.Turnover = clientDto.Turnover ?? clientRto.Turnover;

            await context.SaveChangesAsync();

            return clientRto;
        }

        public async Task<ImportResultBlo> Import(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream); // открываем файл из потока
            var worksheet = workbook.Worksheets.First(); // берем первый лист

            var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1; // находим последнюю используемую строку
            var clients = new List<ClientRto>();
            var errors = new List<string>();

            for (int row = 2; row <= lastRow; row++) // начинаем с 2, пропускаем заголовок
            {
                try
                {
                    var client = ParseRow(worksheet, row);
                    clients.Add(client);
                }
                catch (Exception e)
                {
                    errors.Add($"Строка {row}: {e.Message}");
                }
            }

            // Сохраняем пакетами
            int added = 0;
            int updated = 0;

            foreach (var batch in clients.Chunk(500))
            {
                foreach (var client in batch)
                {
                    var existingClient = await context.Clients.FirstOrDefaultAsync(c => c.CardCode == client.CardCode);

                    if (existingClient != null)
                    {
                        existingClient.LastName = client.LastName;
                        existingClient.FirstName = client.FirstName;
                        existingClient.SurName = client.SurName;
                        existingClient.PhoneMobile = client.PhoneMobile;
                        existingClient.Email = client.Email;
                        existingClient.GenderId = client.GenderId;
                        existingClient.Birthday = client.Birthday;
                        existingClient.City = client.City;
                        existingClient.Pincode = client.Pincode;
                        existingClient.Bonus = client.Bonus;
                        existingClient.Turnover = client.Turnover;

                        updated++;
                    }
                    else
                    {
                        context.Clients.Add(client);
                        added++;
                    }
                }
                await context.SaveChangesAsync();
            }
            return new ImportResultBlo
            {
                Added = added,
                Updated = updated,
                Errors = errors
            };
        }

        private ClientRto ParseRow(IXLWorksheet worksheet, int row)
        {
            return new ClientRto
            {
                CardCode = Extensions.GetLong(worksheet, row, 1),
                LastName = Extensions.GetString(worksheet, row, 2),
                FirstName = Extensions.GetString(worksheet, row, 3),
                SurName = Extensions.GetString(worksheet, row, 4),
                PhoneMobile = Extensions.GetString(worksheet, row, 5),
                Email = Extensions.GetString(worksheet, row, 6),
                GenderId = Extensions.ParseGender(Extensions.GetString(worksheet, row, 7)),
                Birthday = Extensions.ParseBirthday(Extensions.GetString(worksheet, row, 8)),
                City = Extensions.GetString(worksheet, row, 9),
                Pincode = Extensions.GetString(worksheet, row, 10),
                Bonus = Extensions.GetDecimal(worksheet, row, 11),
                Turnover = Extensions.GetDecimal(worksheet, row, 12)
            };
        }
    }
}
