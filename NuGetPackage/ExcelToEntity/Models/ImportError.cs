namespace ExcelToEntity.Models
{
    public class ImportError
    {
        public int RowNumber { get; set; }
        public string ColumnName { get; set; }
        public string Message { get; set; }
    }
}
