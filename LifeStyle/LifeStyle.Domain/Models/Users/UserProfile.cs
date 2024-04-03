namespace LifeStyle.LifeStyle.Domain.Models.Users
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public UserProfile(int id, string email, string phoneNumber, double height, double weight)
        {
            Id = id;
            Email = email;
            PhoneNumber = phoneNumber;
            Height = height;
            Weight = weight;
        }
    }

}
