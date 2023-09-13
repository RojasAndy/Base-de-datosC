//Librerias del ADO .NET
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using Base_de_datos;

class Program
{
    // Cadena de conexión a la base de datos
    public static string connectionString = "Data Source=LAB1504-02\\SQLEXPRESS;Initial Catalog=Tecsup2023DB;User ID=usuario3000;Password=bande3000";


    static void Main()
    {
        /*
        #region FormaDesconectada
        //Datatable
        DataTable dataTable = ListarEmpleadosDataTable();
       
       
       Console.WriteLine("Lista de Empleados:");
       foreach (DataRow row in dataTable.Rows)
       {
           Console.WriteLine($"ID: {row["IdEmpleado"]}, Nombre: {row["Nombre"]}, Cargo: {row["Cargo"]}");
       }
        #endregion
       */



        #region FormaConectada
        //Datareader
        List<Estudiante> empleados = ListarEstudiantesListaObjetos();
        foreach (var item in empleados)
        {
            Console.WriteLine($"ID: {item.Id}, Nombre: {item.Nombre}, Apellido: {item.Apellido}");
        }
        #endregion


    }

    //De forma desconectada
    private static DataTable ListarEstudiantesDataTable()
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
    private static List<Estudiante> ListarEstudiantesListaObjetos()
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


}