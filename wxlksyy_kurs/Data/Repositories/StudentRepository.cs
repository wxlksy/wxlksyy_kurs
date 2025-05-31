using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    public class StudentRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public StudentRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();
            var query = "SELECT student_id, full_name, group_name, student_number FROM Students";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                students.Add(new Student
                {
                    Id = Convert.ToInt32(row["student_id"]),
                    FullName = row["full_name"].ToString(),
                    GroupName = row["group_name"].ToString(),
                    StudentNumber = row["student_number"].ToString()
                });
            }

            return students;
        }

        public int Add(Student student)
        {
            // 1. Получаем максимальный существующий ID
            int maxId = Convert.ToInt32(_databaseHelper.ExecuteScalar(
                "SELECT ISNULL(MAX(student_id), 0) FROM Students"));

            // 2. Вычисляем новый ID
            int newId = maxId + 1;

            // 3. Выполняем вставку с явным указанием ID
            var query = @"INSERT INTO Students 
                        (student_id, full_name, group_name, student_number) 
                        VALUES (@id, @fullName, @groupName, @studentNumber)";

            var parameters = new[]
            {
                new SqlParameter("@id", newId),
                new SqlParameter("@fullName", student.FullName ?? string.Empty),
                new SqlParameter("@groupName", student.GroupName ?? string.Empty),
                new SqlParameter("@studentNumber", student.StudentNumber ?? string.Empty)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);

            // 4. Возвращаем новый ID
            return newId;
        }

        public void Update(Student student)
        {
            var query = @"UPDATE Students 
                        SET full_name = @fullName, 
                            group_name = @groupName, 
                            student_number = @studentNumber
                        WHERE student_id = @id";

            var parameters = new[]
            {
                new SqlParameter("@id", student.Id),
                new SqlParameter("@fullName", student.FullName ?? string.Empty),
                new SqlParameter("@groupName", student.GroupName ?? string.Empty),
                new SqlParameter("@studentNumber", student.StudentNumber ?? string.Empty)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Students WHERE student_id = @id";
            var parameters = new[] { new SqlParameter("@id", id) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}