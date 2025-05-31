using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с пересдачами
    /// </summary>
    public class RetakeRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public RetakeRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Retake> GetAll()
        {
            var retakes = new List<Retake>();
            var query = @"SELECT retake_id, student_id, subject_id, retake_date, 
                          remaining_debts, teacher_id, classroom_id, status_id 
                          FROM Retakes";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                retakes.Add(new Retake
                {
                    Id = Convert.ToInt32(row["retake_id"]),
                    StudentId = Convert.ToInt32(row["student_id"]),
                    SubjectId = Convert.ToInt32(row["subject_id"]),
                    RetakeDate = Convert.ToDateTime(row["retake_date"]),
                    RemainingDebts = Convert.ToInt32(row["remaining_debts"]),
                    TeacherId = Convert.ToInt32(row["teacher_id"]),
                    ClassroomId = Convert.ToInt32(row["classroom_id"]),
                    StatusId = Convert.ToInt32(row["status_id"])
                });
            }

            return retakes;
        }

        public void Add(Retake retake)
        {
            var query = @"INSERT INTO Retakes (student_id, subject_id, retake_date, 
                          remaining_debts, teacher_id, classroom_id, status_id) 
                          VALUES (@studentId, @subjectId, @retakeDate, @remainingDebts, 
                          @teacherId, @classroomId, @statusId)";

            var parameters = new[]
            {
                new SqlParameter("@studentId", retake.StudentId),
                new SqlParameter("@subjectId", retake.SubjectId),
                new SqlParameter("@retakeDate", retake.RetakeDate),
                new SqlParameter("@remainingDebts", retake.RemainingDebts),
                new SqlParameter("@teacherId", retake.TeacherId),
                new SqlParameter("@classroomId", retake.ClassroomId),
                new SqlParameter("@statusId", retake.StatusId)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void UpdateStatus(int retakeId, int statusId)
        {
            var query = "UPDATE Retakes SET status_id = @statusId WHERE retake_id = @retakeId";
            var parameters = new[]
            {
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@retakeId", retakeId)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}