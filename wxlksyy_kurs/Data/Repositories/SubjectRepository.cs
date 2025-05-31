using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с предметами
    /// </summary>
    public class SubjectRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public SubjectRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Subject> GetAll()
        {
            var subjects = new List<Subject>();
            var query = "SELECT subject_id, subject_name FROM Subjects";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                subjects.Add(new Subject
                {
                    Id = Convert.ToInt32(row["subject_id"]),
                    Name = row["subject_name"].ToString()
                });
            }

            return subjects;
        }

        public void Add(Subject subject)
        {
            var query = "INSERT INTO Subjects (subject_name) VALUES (@name)";
            var parameters = new[] { new SqlParameter("@name", subject.Name) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Subjects WHERE subject_id = @id";
            var parameters = new[] { new SqlParameter("@id", id) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}