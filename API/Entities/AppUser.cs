namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        //We are storing a password hash and salt in database. These will be used to verify whath the user enters on from end.
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}