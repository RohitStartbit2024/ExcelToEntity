using ExcelToEntity;
using ExcelToEntity.Models;

class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patients.xlsx");

        var importer = new ExcelImporter();
        var result = importer.Import<Patient>(filePath);

        Console.WriteLine("=== Imported Patients ===");
        foreach (var patient in result.Data)
        {
            Console.WriteLine($"{patient.FirstName} {patient.LastName}, Age: {patient.Age}, Email: {patient.Email}");
        }

        Console.WriteLine("\n=== Errors ===");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Row {error.RowNumber}, Column {error.ColumnName}: {error.Message}");
        }
    }
}
