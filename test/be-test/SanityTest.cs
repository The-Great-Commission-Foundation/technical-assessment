namespace BackendTesting
{
    using System.Threading.Tasks;
    using NUnit.Framework.Legacy;
    using Microsoft.Data.Sqlite;

    public class SanityTest
    {
        #region Private Variables

        private SqliteConnection _connection;
        private string _donorName;
        private double _donationAmount;
        private DateTime _donationDate;
        private HttpClient _httpClient;

        #endregion

        #region Constants

        const string BASE_URL = "http://localhost:5050";
        const int DB_ID = 1;
        const string DB_NAME = "John Doe";
        const double DB_AMOUNT = 100.50;
        const string DB_DATE = "2025-09-05";

        #endregion

        #region Public Methods

        public async Task<string> SendGetRequest(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode(); // Throws an exception if not a success status code
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendPostRequest(string endpoint, HttpContent content)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        [SetUp]
        public void Setup()
        {
            _donorName = "John Doe";
            _donationAmount = 100.00;
            _donationDate = new DateTime(2025, 09, 03);

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BASE_URL);

            _connection = new SqliteConnection("Data Source=../../../../../db/mydatabase.db");
        }

        [Test, Order(1)]
        public async Task Test_PingCheck()
        {
            ClassicAssert.NotNull(await SendGetRequest("/ping"));
        }

        [Test, Order(2)]
        public void Test_InsertToDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                string sql = "INSERT INTO donations (id, donor_name, amount, date) VALUES (@id, @donorName, @amount, @date)";
                var command = new SqliteCommand(sql, _connection);

                command.Parameters.AddWithValue("@id", DB_ID);
                command.Parameters.AddWithValue("@donorName", DB_NAME);
                command.Parameters.AddWithValue("@amount", DB_AMOUNT);
                command.Parameters.AddWithValue("@date", DB_DATE);

                int insertedRows = command.ExecuteNonQuery();
                Assert.That(insertedRows == 1);
            });
        }

        [Test, Order(3)]
        public void Test_ReadFromDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                var command = _connection.CreateCommand();
                command.CommandText = "SELECT id, donor_name, amount, date FROM donations WHERE id = "+DB_ID;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        double amount = reader.GetDouble(2);
                        string date = reader.GetString(3);

                        Assert.That(id == DB_ID);
                        Assert.That(name == DB_NAME);
                        Assert.That(amount == DB_AMOUNT);
                        Assert.That(date == DB_DATE);
                    }
                }
            });
        }

        [Test, Order(4)]
        public void Test_DeleteFromDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                string sql = "DELETE FROM donations WHERE id=@id AND donor_name=@donorName AND amount=@amount AND date=@date";
                var command = new SqliteCommand(sql, _connection);

                command.Parameters.AddWithValue("@id", DB_ID);
                command.Parameters.AddWithValue("@donorName", DB_NAME);
                command.Parameters.AddWithValue("@amount", DB_AMOUNT);
                command.Parameters.AddWithValue("@date", DB_DATE);

                int deletedRows = command.ExecuteNonQuery();
                Assert.That(deletedRows == 1);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _connection?.Dispose();
            _httpClient?.Dispose();
        }
    }
}