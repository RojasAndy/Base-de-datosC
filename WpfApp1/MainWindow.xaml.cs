using Base_de_datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using Base_de_datos;

namespace WpfApp1
{
    /// <summary>
    /// 
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public  string connectionString = "Data Source=LAB1504-02\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;User ID=usuario3000;Password=bande3000";


        public MainWindow()
        {

            InitializeComponent();
            McDataGrid.ItemsSource = ListarEstudiantesListaObjetos();
            txtSearch.TextChanged += TxtSearch_TextChanged;

        }
        /// List of Authors
        /// </summary>
        /// <returns></returns>
        //De forma desconectada
        private  DataTable ListarEstudiantesDataTable()
        {


            // Crear un DataTable para almacenar los resultados
            DataTable dataTable = new DataTable();
            // Crear una conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para seleccionar datos
                string query = "SELECT * FROM Students";

                // Crear un adaptador de datos
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);



                // Llenar el DataTable con los datos de la consulta
                adapter.Fill(dataTable);

                // Cerrar la conexión
                connection.Close();

            }
            return dataTable;
        }
        //De forma conectada
        private  List<Estudiante> ListarEstudiantesListaObjetos()
        {
            List<Estudiante> estudiantes = new List<Estudiante>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para seleccionar datos
                string query = "SELECT StudentID,FirstName,LastName FROM Students";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Verificar si hay filas
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Lista de Estudiantes:");
                            while (reader.Read())
                            {
                                // Leer los datos de cada fila

                                estudiantes.Add(new Estudiante
                                {
                                    Id = (int)reader["StudentID"],
                                    Nombre = reader["FirstName"].ToString(),
                                    Apellido = reader["LastName"].ToString()
                                });

                            }
                        }
                    }
                }

                // Cerrar la conexión
                connection.Close();


            }
            return estudiantes;

        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Construct the SQL query to search for students whose names or last names contain the search text
                string query = "SELECT StudentID, FirstName, LastName FROM Students WHERE FirstName LIKE @searchText OR LastName LIKE @searchText";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchText", "%" + searchText + "%"); // Use "%" to perform a partial match

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataGrid to the results of the SQL query
                        McDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }

                connection.Close();
            }
        }
    }
}
