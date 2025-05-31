using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using wxlksyy_kurs.Data.Models;

namespace wxlksyy_kurs.Data.Repositories
{
    /// <summary>
    /// Репозиторий для работы с аудиториями
    /// </summary>
    public class ClassroomRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public ClassroomRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Classroom> GetAll()
        {
            var classrooms = new List<Classroom>();
            var query = "SELECT classroom_id, room_number, building FROM Classrooms";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                classrooms.Add(new Classroom
                {
                    Id = Convert.ToInt32(row["classroom_id"]),
                    RoomNumber = row["room_number"].ToString(),
                    Building = row["building"].ToString()
                });
            }

            return classrooms;
        }

        public void Add(Classroom classroom)
        {
            var query = "INSERT INTO Classrooms (room_number, building) VALUES (@roomNumber, @building)";
            var parameters = new[]
            {
                new SqlParameter("@roomNumber", classroom.RoomNumber),
                new SqlParameter("@building", classroom.Building)
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Classrooms WHERE classroom_id = @id";
            var parameters = new[] { new SqlParameter("@id", id) };
            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
