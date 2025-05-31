namespace wxlksyy_kurs.Data.Models
{
    /// <summary>
    /// Модель академической задолженности
    /// </summary>
    public class Debt
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string Description { get; set; }
    }
}