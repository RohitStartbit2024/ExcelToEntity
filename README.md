# ExcelToEntity 📊 ➡️ 🗂️

**ExcelToEntity** is a lightweight **.NET library** that helps you
easily import data from **Excel files** (`.xlsx`) into strongly-typed C#
entities.\
It uses **ClosedXML** internally for reading Excel files and provides
detailed error handling for invalid or missing data.

------------------------------------------------------------------------

## 🚀 Features

-   Import Excel data directly into strongly typed entities (`T`).
-   Automatically maps Excel columns to entity properties by name.
-   Error handling with row/column details for invalid conversions.
-   Supports validation (e.g., numeric parsing, required fields).
-   Works with **.NET 6, .NET 7, .NET 8**.

------------------------------------------------------------------------

## 📦 Installation

Install from **NuGet**:

``` bash
dotnet add package ExcelToEntity
```

Or via **Package Manager Console**:

``` powershell
Install-Package ExcelToEntity
```

------------------------------------------------------------------------

## 📖 Usage

### 1️⃣ Define Your Entity

``` csharp
public class Patient
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}
```

### 2️⃣ Import from Excel

``` csharp
using ExcelToEntity;

var importer = new ExcelImporter();
var result = importer.Import<Patient>("Patients.xlsx");

// Print imported data
foreach (var patient in result.Entities)
{
    Console.WriteLine($"{patient.Name}, Age: {patient.Age}, Email: {patient.Email}");
}

// Print errors (if any)
foreach (var error in result.Errors)
{
    Console.WriteLine($"Row {error.Row}, Column {error.Column}: {error.Message}");
}
```

------------------------------------------------------------------------

### Running the Sample Project

1. Open the solution in Visual Studio.  
2. Right-click on **SampleConsole** → **Set as Startup Project**.  
3. Build the solution (`Ctrl+Shift+B`).  
4. Copy your `Patients.xlsx` file into: -SampleConsole/bin/Debug/net8.0/
5. Run the Console App (`F5`).

------------------------------------------------------------------------
## 🖥️ Sample Console App

The repository also contains a **SampleConsole** project that
demonstrates usage.

**Example Output:**

    === Imported Patients ===
    John Doe, Age: 30, Email: john@example.com
    Jane Smith, Age: 25, Email: jane@example.com
    Sara Lee, Age: 40, Email: sara@example.com

    === Errors ===
    Row 4, Column Age: Invalid integer 'abc'

------------------------------------------------------------------------

## 📂 Project Structure

    ExcelToEntity/
    ├── ExcelToEntity/        # Class Library (NuGet package)
    │   ├── ExcelImporter.cs
    │   ├── ImportResult.cs
    │   └── ...
    ├── SampleConsole/        # Demo Console App
    │   ├── Program.cs
    │   └── Patients.xlsx
    └── README.md             # Project documentation

------------------------------------------------------------------------

## ⚖️ License

This project is licensed under the **MIT License** -- free to use,
modify, and distribute.
