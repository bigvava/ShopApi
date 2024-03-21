using System.Data;

namespace ShopApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public ICollection<UsersRole> UsersRoles { get; set; }
    }
}
