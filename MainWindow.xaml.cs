using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace HotelManagement
{
    public partial class MainWindow : Window
    {
        private readonly Database _database;
        private int _selectedRoomId;
        private Room _selectedRoom;
        private Employee _selectedEmployee;

        public MainWindow()
        {
            InitializeComponent();
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "HotelManagement.db3");
            _database = new Database(dbPath);
            LoadRooms();
        }

        private async void LoadRooms()
        {
            RoomsDataGrid.ItemsSource = await _database.GetRoomsAsync();
        }

        private async void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsDataGrid.SelectedItem is Room selectedRoom)
            {
                _selectedRoom = selectedRoom;
                _selectedRoomId = selectedRoom.Id;
                ClientsDataGrid.ItemsSource = await _database.GetClientsAsync(_selectedRoomId);
                EmployeesDataGrid.ItemsSource = await _database.GetEmployeesAsync(_selectedRoomId);
                RoomNameTextBox.Text = selectedRoom.Name;
            }
        }

        private async void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var room = new Room { Name = RoomNameTextBox.Text };
            await _database.SaveRoomAsync(room);
            LoadRooms();
        }

        private async void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null)
            {
                _selectedRoom.Name = RoomNameTextBox.Text;
                await _database.SaveRoomAsync(_selectedRoom);
                LoadRooms();
            }
        }

        private async void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null)
            {
                await _database.DeleteRoomAsync(_selectedRoom);
                LoadRooms();
            }
        }

        private async void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomId > 0)
            {
                var employee = new Employee { Name = EmployeeNameTextBox.Text, RoomId = _selectedRoomId };
                await _database.SaveEmployeeAsync(employee);
                EmployeesDataGrid.ItemsSource = await _database.GetEmployeesAsync(_selectedRoomId);
            }
        }

        private async void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee != null)
            {
                _selectedEmployee.Name = EmployeeNameTextBox.Text;
                await _database.SaveEmployeeAsync(_selectedEmployee);
                EmployeesDataGrid.ItemsSource = await _database.GetEmployeesAsync(_selectedRoomId);
            }
        }

        private async void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee != null)
            {
                await _database.DeleteEmployeeAsync(_selectedEmployee);
                EmployeesDataGrid.ItemsSource = await _database.GetEmployeesAsync(_selectedRoomId);
            }
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem is Employee selectedEmployee)
            {
                _selectedEmployee = selectedEmployee;
                EmployeeNameTextBox.Text = selectedEmployee.Name;
            }
        }
    }
}
