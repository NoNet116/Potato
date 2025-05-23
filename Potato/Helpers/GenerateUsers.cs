using Potato.DbContext.Models.Entity;
using System.Text.Json;

namespace Potato.Helpers
{
    public class RandomUserResponse
    {
        public List<RandomUserResult> results { get; set; }
    }

    public class RandomUserResult
    {
        public Name name { get; set; }
        public string email { get; set; }
        public Dob dob { get; set; }
        public Picture picture { get; set; }
    }

    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Dob
    {
        public string date { get; set; }
        public int age { get; set; }
    }

    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }


    public class GenerateUsers
    {
        private readonly HttpClient _httpClient;

        public GenerateUsers(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> PopulateAsync(int count)
        {
            var users = new List<User>();
            var url = $"https://randomuser.me/api/?results={count}&nat=us,gb&inc=name,email,picture,dob";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return users;

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RandomUserResponse>(content);

            int i = 0;
            foreach (var result in data.results)
            {
                users.Add(new User
                {
                    UserName = result.name.first + result.name.last,
                    Email = result.email,
                    FirstName = result.name.first,
                    LastName = result.name.last,
                    BirthDate = DateTime.Parse(result.dob.date),
                    Image = result.picture.medium,
                    Status = "Онлайн",
                    About = "Сгенерирован через randomuser.me"
                });

                i++;
            }

            return users;
        }
    }
}
