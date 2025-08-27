namespace ExcelToEntity.Models
{
    public class ImportResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
        public bool HasErrors => Errors.Count > 0;
    }
}
