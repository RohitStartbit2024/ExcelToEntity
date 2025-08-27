using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using ExcelToEntity.Models;

namespace ExcelToEntity
{
    public class ExcelImporter
    {
        public ImportResult<T> Import<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Excel file not found: {filePath}");

            var result = new ImportResult<T>();
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.First();

            // Read headers (first row)
            var headerRow = worksheet.FirstRowUsed();
            var headers = headerRow.Cells().Select((c, i) => new { Index = i + 1, Name = c.GetString() }).ToList();

            // Map model properties to headers
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => new
                {
                    Property = p,
                    HeaderName = p.GetCustomAttribute<ExcelHeaderAttribute>()?.HeaderName ?? p.Name
                })
                .ToList();

            var headerMap = properties
                .Select(p => new
                {
                    p.Property,
                    ColumnIndex = headers.FirstOrDefault(h => string.Equals(h.Name, p.HeaderName, StringComparison.OrdinalIgnoreCase))?.Index
                })
                .ToList();

            // Process rows
            foreach (var row in worksheet.RowsUsed().Skip(1)) // skip header
            {
                var entity = new T();
                bool hasError = false;

                foreach (var map in headerMap)
                {
                    if (map.ColumnIndex == null) continue; // header not found, skip

                    var cell = row.Cell(map.ColumnIndex.Value);
                    var cellValue = cell.Value;

                    try
                    {
                        object value = null;
                        if (!cellValue.IsBlank)
                        {
                            value = ConvertCell(cellValue, map.Property.PropertyType);
                        }
                        map.Property.SetValue(entity, value);
                    }
                    catch (Exception ex)
                    {
                        hasError = true; // 👈 mark error so row won’t be added
                        result.Errors.Add(new ImportError
                        {
                            RowNumber = row.RowNumber(),
                            ColumnName = map.Property.Name,
                            Message = $"Failed to convert '{cell.Value.ToString()}' to {map.Property.PropertyType.Name}: {ex.Message}"
                        });
                    }
                }

                if (!hasError) // 👈 only add valid rows
                    result.Data.Add(entity);
            }


            return result;
        }

        private object? ConvertCell(XLCellValue cellValue, Type targetType)
        {
            // Treat blank cells as null
            if (cellValue.IsBlank)
                return null;

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                switch (underlyingType)
                {
                    case Type t when t == typeof(string):
                        return cellValue.Type switch
                        {
                            XLDataType.Number => cellValue.GetNumber().ToString(),
                            XLDataType.DateTime => cellValue.GetDateTime().ToString("yyyy-MM-dd"),
                            XLDataType.Boolean => cellValue.GetBoolean().ToString(),
                            _ => cellValue.GetText()
                        };

                    case Type t when t.IsEnum:
                        return Enum.Parse(underlyingType, cellValue.GetText(), true);

                    case Type t when t == typeof(DateTime):
                        if (cellValue.Type == XLDataType.DateTime)
                            return cellValue.GetDateTime();
                        return DateTime.Parse(cellValue.GetText());

                    case Type t when t == typeof(int):
                        if (cellValue.Type == XLDataType.Number)
                            return (int)cellValue.GetNumber();
                        return int.Parse(cellValue.GetText());

                    case Type t when t == typeof(double):
                        if (cellValue.Type == XLDataType.Number)
                            return cellValue.GetNumber();
                        return double.Parse(cellValue.GetText());

                    case Type t when t == typeof(bool):
                        if (cellValue.Type == XLDataType.Boolean)
                            return cellValue.GetBoolean();
                        return bool.Parse(cellValue.GetText());

                    default:
                        // fallback for other types
                        return Convert.ChangeType(cellValue.GetText(), underlyingType);
                }
            }
            catch (Exception ex)
            {
                throw new FormatException(
                    $"Failed to convert '{cellValue.GetText()}' to {underlyingType.Name}: {ex.Message}"
                );
            }
        }



    }
}
