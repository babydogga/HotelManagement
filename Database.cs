using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelManagement
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Room>().Wait();
            _database.CreateTableAsync<Client>().Wait();
            _database.CreateTableAsync<Employee>().Wait();

            // Przykładowe dane
            if (_database.Table<Room>().CountAsync().Result == 0)
            {
                _database.InsertAllAsync(new[]
                {
                    new Room { Name = "Room 101" },
                    new Room { Name = "Room 102" },
                    new Room { Name = "Room 103" },
                    new Room { Name = "Room 104" },
                    new Room { Name = "Room 105" }
                }).Wait();
            }

            if (_database.Table<Client>().CountAsync().Result == 0)
            {
                _database.InsertAllAsync(new[]
                {
                    new Client { Name = "Client A", RoomId = 1 },
                    new Client { Name = "Client B", RoomId = 1 },
                    new Client { Name = "Client C", RoomId = 2 },
                    new Client { Name = "Client D", RoomId = 3 },
                    new Client { Name = "Client E", RoomId = 4 }
                }).Wait();
            }

            if (_database.Table<Employee>().CountAsync().Result == 0)
            {
                _database.InsertAllAsync(new[]
                {
                    new Employee { Name = "Employee 1", RoomId = 1 },
                    new Employee { Name = "Employee 2", RoomId = 1 },
                    new Employee { Name = "Employee 3", RoomId = 2 },
                    new Employee { Name = "Employee 4", RoomId = 3 },
                    new Employee { Name = "Employee 5", RoomId = 4 }
                }).Wait();
            }
        }

        // Metody dla tabeli Room
        public Task<List<Room>> GetRoomsAsync() => _database.Table<Room>().ToListAsync();
        public Task<int> SaveRoomAsync(Room room) => room.Id != 0 ? _database.UpdateAsync(room) : _database.InsertAsync(room);
        public Task<int> DeleteRoomAsync(Room room) => _database.DeleteAsync(room);

        // Metody dla tabeli Client
        public Task<List<Client>> GetClientsAsync(int roomId) => _database.Table<Client>().Where(c => c.RoomId == roomId).ToListAsync();
        public Task<int> SaveClientAsync(Client client) => _database.InsertAsync(client);
        public Task<int> DeleteClientAsync(Client client) => _database.DeleteAsync(client);

        // Metody dla tabeli Employee
        public Task<List<Employee>> GetEmployeesAsync(int roomId) => _database.Table<Employee>().Where(e => e.RoomId == roomId).ToListAsync();
        public Task<int> SaveEmployeeAsync(Employee employee) => employee.Id != 0 ? _database.UpdateAsync(employee) : _database.InsertAsync(employee);
        public Task<int> DeleteEmployeeAsync(Employee employee) => _database.DeleteAsync(employee);
    }

    // Klasa Room
    public class Room
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // Klasa Client
    public class Client
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
    }

    // Klasa Employee
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
    }
}
