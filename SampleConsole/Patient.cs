using ExcelToEntity;

public class Patient
{
    [ExcelHeader("First Name")]
    public string FirstName { get; set; }

    [ExcelHeader("Last Name")]
    public string LastName { get; set; }

    [ExcelHeader("Age")]
    public int Age { get; set; }

    [ExcelHeader("Email")]
    public string Email { get; set; }
}
