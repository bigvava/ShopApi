namespace ShopApi.Entities
{
    public class UsersRole
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
