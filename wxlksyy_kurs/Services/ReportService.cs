using System;
using System.Data;
using wxlksyy_kurs.Data;

namespace wxlksyy_kurs.Services
{
    /// <summary>
    /// Сервис для генерации отчетов
    /// </summary>
    public class ReportService
    {
        private readonly DatabaseHelper _databaseHelper;

        public ReportService()
        {
            _databaseHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Генерирует отчет по задолженностям
        /// </summary>
        public DataTable GenerateDebtsReport()
        {
            var query = @"SELECT s.full_name, s.group_name, sub.subject_name, d.debt_description
                         FROM Debts d
                         JOIN Students s ON d.student_id = s.student_id
                         JOIN Subjects sub ON d.subject_id = sub.subject_id";

            return _databaseHelper.ExecuteQuery(query);
        }

        /// <summary>
        /// Генерирует отчет по пересдачам
        /// </summary>
        public DataTable GenerateRetakesReport()
        {
            var query = @"SELECT s.full_name, sub.subject_name, r.retake_date, 
                         t.full_name AS teacher_name, c.room_number, c.building
                         FROM Retakes r
                         JOIN Students s ON r.student_id = s.student_id
                         JOIN Subjects sub ON r.subject_id = sub.subject_id
                         JOIN Teachers t ON r.teacher_id = t.teacher_id
                         JOIN Classrooms c ON r.classroom_id = c.classroom_id";

            return _databaseHelper.ExecuteQuery(query);
        }
    }
}