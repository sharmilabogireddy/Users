using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Users.Dto;
using Users.Model;

namespace Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        [HttpPost]
        [Route("retrieveUsers")]
        public async Task<IActionResult> RetrieveUsers([FromBody] List<string> UserNames)
        {
            if(UserNames == null || UserNames.Count == 0)
            {
                return BadRequest(new { message = "Please proivde list of usernames." });
            }
            try
            {
                // Using regular HTTP Client API
                var client = new HttpClient();
                // As per the GitHub API, 'User-Agent' is mandatory to pass in header
                client.DefaultRequestHeaders.Add("User-Agent", "Capital-Transport");

                var users = new List<UserDto>();
                UserDto UserDto;

                // Remove any duplicate usernames from the input (case-insensitive)
                var DistinctUserNames = UserNames.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                GitHubUser User = null;
                for (int i = 0; i < DistinctUserNames.Count; i++)
                {
                    // Retreive the basic user information from GitHub
                    User = await GetGitHubUserAsync(client, DistinctUserNames[i]);
                    if (User != null)
                    {
                        UserDto = new UserDto();
                        UserDto.Name = User.Name;
                        UserDto.Login = User.Login;
                        UserDto.Company = User.Company;
                        UserDto.NumberOfFollowers = User.Followers;
                        UserDto.NumberOfPublicRepositories = User.Public_Repos;
                        if (User.Followers > 0)
                        {
                            UserDto.AverageNumberOfFollowersPerPublicRepository = User.Followers / User.Public_Repos;
                        }
                        users.Add(UserDto);
                    }
                }

                // Sort the users list based on the 'Name'
                // If name is null, then show at the bottom of the list
                var SortedUsers = users
                    .OrderByDescending(user => user.Name != null)
                    .ThenBy(user => user.Name).ToList();

                return Ok(SortedUsers);
            } catch(Exception e)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
            
        }

        /**
         * This method is used to retreive user information from GitHub
         * using GitHub Public API.
         * 
         */
        static async Task<GitHubUser> GetGitHubUserAsync(HttpClient client, string UserName)
        {
            GitHubUser User = null;
            var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = await client.GetAsync("https://api.github.com/users/" + UserName);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<GitHubUser>(content, _options);
            }
            return User;
        }
    }
}
