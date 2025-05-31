using System;

namespace wxlksyy_kurs.Data.Models
{
    /// <summary>
    /// Модель пересдачи
    /// </summary>
    public class Retake
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime RetakeDate { get; set; }
        public int RemainingDebts { get; set; }
        public int TeacherId { get; set; }
        public int ClassroomId { get; set; }
        public int StatusId { get; set; }
    }
}
