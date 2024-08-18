using Android.App;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;

namespace HEFE
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Referencias a los elementos de la UI
            EditText usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText)!;
            EditText passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText)!;
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
            TextView messageTextView = FindViewById<TextView>(Resource.Id.messageTextView);
            TextView registerLink = FindViewById<TextView>(Resource.Id.registerLink);

            // Configurar evento de clic para el botón de login
            loginButton.Click += (sender, e) =>
            {
                string username = usernameEditText.Text;
                string password = passwordEditText.Text;

                // Verificar credenciales en la base de datos
                bool isValidUser = CheckCredentials(username, password);

                if (isValidUser)
                {
                    messageTextView.Text = "Inicio de sesión exitoso";
                    messageTextView.SetTextColor(Android.Graphics.Color.Green);
                }
                else
                {
                    messageTextView.Text = "Usuario o contraseña incorrectos";
                }
            };

            // Configurar evento de clic para el enlace de registro
            registerLink.Click += (sender, e) =>
            {
                StartActivity(typeof(RegisterActivity));
            };
        }

        private bool CheckCredentials(string username, string password)
        {
            string connectionString = "Server=localhost;Database=ACTIVEAURA;User ID=your_username;Password=your_password;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para verificar el usuario
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int userCount))
                        {
                            return userCount > 0;
                        }

                        return false;
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

