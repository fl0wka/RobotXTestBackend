using System.Globalization;
using ClosedXML.Excel;
using RobotXTest.DataAccess.Core.Enums;

namespace RobotXTest.Shared.Helpers
{
    public static class Extensions
    {
        public static long GetLong(IXLWorksheet ws, int row, int col)
        {
            var cell = ws.Cell(row, col);

            var value = cell.HasFormula
                ? cell.CachedValue.ToString()
                : cell.GetString();

            return long.Parse(value);
        }

        public static string? GetString(IXLWorksheet ws, int row, int col)
        {
            var cell = ws.Cell(row, col);

            if (cell.IsEmpty()) return null;

            var value = cell.HasFormula
                ? cell.CachedValue.ToString()
                : cell.GetString();

            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public static decimal? GetDecimal(IXLWorksheet ws, int row, int col)
        {
            var raw = GetString(ws, row, col);
            if (raw == null) return null;

            return decimal.TryParse(raw, CultureInfo.InvariantCulture, out var result)
                ? result
                : null;
        }

        public static GenderType? ParseGender(string? raw)
        {
            return raw?.ToLower() switch
            {
                "муж" => GenderType.MALE,
                "жен" => GenderType.FEMALE,
                _ => null
            };
        }

        public static DateTime? ParseBirthday(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            string[] formats = { "d.M.yyyy", "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy" };

            return DateTime.TryParseExact(raw, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)
                ? result
                : null;
        }
    }
}
