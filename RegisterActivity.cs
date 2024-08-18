using Android.App;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace HEFE
{
    [Activity(Label = "Register")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);

            // Referencias a los elementos de la UI
            EditText usernameEditText = FindViewById<EditText>(Resource.Id.registerUsernameEditText)!;
            EditText passwordEditText = FindViewById<EditText>(Resource.Id.registerPasswordEditText)!;
            Button registerButton = FindViewById<Button>(Resource.Id.registerButton);
            TextView messageTextView = FindViewById<TextView>(Resource.Id.registerMessageTextView);

            // Configurar evento de clic para el botón de registro
            registerButton.Click += (sender, e) =>
            {
                string username = usernameEditText.Text;
                string password = passwordEditText.Text;

                // Registrar el usuario en la base de datos
                bool isRegistered = RegisterUser(username, password);

                if (isRegistered)
                {
                    messageTextView.Text = "Registro exitoso"; // Puedes usar Resource.String.register_success si prefieres
                    messageTextView.SetTextColor(Android.Graphics.Color.Green);
                }
                else
                {
                    messageTextView.Text = "Error al registrar el usuario"; // Puedes usar Resource.String.register_failure si prefieres
                }
            };
        }

        private bool RegisterUser(string username, string password)
        {
            string connectionString = "Server=Fatima;Database=ACTIVEAURA;User ID=sa;Password=1234;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para registrar un nuevo usuario
                    string query = "INSERT INTO users (username, password) VALUES (@username, @password)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error de conexión: " + ex.Message, ToastLength.Long).Show();
                    return false;
                }
            }
        }
    }
}
