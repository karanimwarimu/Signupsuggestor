using BCrypt.Net;
using  Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace LOGGINN
{
    public class CONTROLLER
    {
        
        private static string conn = "Data Source=localhost;Initial Catalog= playtool;Integrated Security=True";
      // private static string conn = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;



        public void SaveUserData(string username, string firstname, string lastname, string email, string pasowrd, string userrole)
        {
            string sqlstringsave = " INSERT INTO DummieLogIn (userName , firstName , lastName , UserEmail , userPassword , role ) " +
                                "   VALUES ( @username , @firstname , @lastname , @useremail , @userpassword , @userrole ) ";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlstringsave, connection))
                    {
                        // Add parameters to the command

                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@firstname", firstname);
                        command.Parameters.AddWithValue("@lastname", lastname);
                        command.Parameters.AddWithValue("@useremail", email);
                        command.Parameters.AddWithValue("@userpassword", pasowrd);
                        command.Parameters.AddWithValue("@userrole", userrole ?? "USER");

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User data saved successfully.");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving user data: " + ex.Message);
            }
        }

        public string SecurePassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool verifyUSer(string username, string password)
        {
            string queryString = " SELECT userPassword FROM DummieLogIn WHERE userName = @username ";
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        string storedPassword = command.ExecuteScalar() as string;
                        if (storedPassword != null)
                        {
                            return BCrypt.Net.BCrypt.Verify(password, storedPassword);
                        }
                        else
                        {
                            MessageBox.Show("Username not found.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while verifying user: " + ex.Message);
                return false;
            }
        }



        public bool verifyEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }
            string sqlstring = " SELECT COUNT(*) FROM DummieLogIn WHERE UserEmail = @Email";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlstring, connection))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        int count = (int)command.ExecuteScalar();
                        return count > 0; // Returns true if email exists
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while checking email: " + ex.Message);
                return false;
            }
        }

        public static bool verifyUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            string queryString = "SELECT COUNT(*) FROM DummieLogIn WHERE userName = @username";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(queryString, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while checking username: " + ex.Message);
                return false;
            }

        }


        public class UsernameGenerator
        {
            private readonly string apiUrl = "https://api.openai.com/v1/chat/completions"; 
            //private readonly string apiKey = "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"; 
            private readonly HttpClient client = new HttpClient();
            string apiKey = ConfigurationManager.AppSettings["OpenAI_API_Key"];
            public async Task<List<string>> SuggestPasswordAsync(string fName, string lName, string prevUsername)
            {
                fName = fName?.Trim().ToUpper();
                lName = lName?.Trim().ToUpper();
                prevUsername = prevUsername?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName))
                {
                    return null;
                }

                string input = $"{fName} {lName} {prevUsername}";
                string generatePrompt = $"Generate five cool one word usernames using the following: {input}";

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                            new { role = "system", content = "You are a creative username generator." },
                            new { role = "user", content = generatePrompt }
                    },
                    n = 1, 
                    temperature = 0.7
                };

                var json = JsonConvert.SerializeObject(requestBody);

                var requestHttp = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                requestHttp.Content = new StringContent(json, Encoding.UTF8, "application/json");
                requestHttp.Headers.Add("Authorization", $"Bearer {apiKey}");

                try
                {
                    var response = await client.SendAsync(requestHttp);
                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"API Error: {response.StatusCode}\n{error}");
                        return null ;
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    // string generatedUsernames; 
                    //return responseObject.choices[0].message.content.ToString();
                    // return generatedUsernames;


                    string content = responseObject.choices[0].message.content.ToString();
 
                    var rawusernames = content
                        .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(line => line.Trim())
                        .Where(line => line.Length > 0 && (line.Any(char.IsLetterOrDigit))) 
                        .Select(line =>
                        {
                          
                            var index = line.IndexOfAny(new[] { '.', '-', ')' });
                            if (index > 0 && index < 4)
                            {
                                return line.Substring(index + 1).Trim();
                            }
                            return line;
                        })
                         .Where(line => line.Length >= 3 && line.Length<8 ) 
                         .Distinct(StringComparer.OrdinalIgnoreCase) 
                         .ToList();

                    List<string> availableUsernames = new List<string>();

                    foreach (string username in rawusernames)
                    {
                        if (!verifyUserName(username))
                        {
                            continue;
                        }
                        availableUsernames.Add(username);
                    }

                    return availableUsernames;

                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while generating usernames: {ex.Message}");
                    return null; 
                }
            }
        }
    }
}
       

