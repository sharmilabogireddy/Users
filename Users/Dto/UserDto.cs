namespace Users.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Company { get; set; }
        public int NumberOfFollowers { get; set; }
        public int NumberOfPublicRepositories { get; set; }
        public int AverageNumberOfFollowersPerPublicRepository { get;set; }

    }
}
