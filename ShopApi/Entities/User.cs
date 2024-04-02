using System.Data;

namespace ShopApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<UsersRole> UsersRoles { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
