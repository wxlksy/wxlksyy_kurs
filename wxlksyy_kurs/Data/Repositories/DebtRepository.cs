using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с задолженностями
    /// </summary>
    public class DebtRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public DebtRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Debt> GetAll()
        {
            var debts = new List<Debt>();
            var query = "SELECT debt_id, student_id, subject_id, debt_description FROM Debts";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                debts.Add(new Debt
                {
                    Id = Convert.ToInt32(row["debt_id"]),
                    StudentId = Convert.ToInt32(row["student_id"]),
                    SubjectId = Convert.ToInt32(row["subject_id"]),
                    Description = row["debt_description"].ToString()
                });
            }

            return debts;
        }

        public int Add(Debt debt)
        {
            // 1. Получаем максимальный существующий ID
            int maxId = Convert.ToInt32(_databaseHelper.ExecuteScalar(
                "SELECT ISNULL(MAX(debt_id), 0) FROM Debts"));

            // 2. Вычисляем новый ID
            int newId = maxId + 1;

            // 3. Выполняем вставку с явным указанием ID
            var query = @"INSERT INTO Debts 
                        (debt_id, student_id, subject_id, debt_description) 
                        VALUES (@id, @studentId, @subjectId, @description)";

            var parameters = new[]
            {
                new SqlParameter("@id", newId),
                new SqlParameter("@studentId", debt.StudentId),
                new SqlParameter("@subjectId", debt.SubjectId),
                new SqlParameter("@description", debt.Description ?? string.Empty)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);

            // 4. Возвращаем новый ID
            return newId;
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Debts WHERE debt_id = @id";
            var parameters = new[] { new SqlParameter("@id", id) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}