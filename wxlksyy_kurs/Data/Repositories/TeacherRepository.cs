using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с преподавателями
    /// </summary>
    public class TeacherRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public TeacherRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Teacher> GetAll()
        {
            var teachers = new List<Teacher>();
            var query = "SELECT teacher_id, full_name, department FROM Teachers";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                teachers.Add(new Teacher
                {
                    Id = Convert.ToInt32(row["teacher_id"]),
                    FullName = row["full_name"].ToString(),
                    Department = row["department"].ToString()
                });
            }

            return teachers;
        }

        public void Add(Teacher teacher)
        {
            var query = "INSERT INTO Teachers (full_name, department) VALUES (@fullName, @department)";
            var parameters = new[]
            {
                new SqlParameter("@fullName", teacher.FullName),
                new SqlParameter("@department", teacher.Department)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Teachers WHERE teacher_id = @id";
            var parameters = new[] { new SqlParameter("@id", id) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
