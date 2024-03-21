namespace ShopApi.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<UsersRole> UsersRoles { get; set; }
    }
}
