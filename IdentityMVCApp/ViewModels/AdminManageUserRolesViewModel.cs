namespace IdentityMVCApp.ViewModels
{
    public class AdminManageUserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<RoleSelection> Roles { get; set; } = new();

        public string SelectedRole { get; set; } = string.Empty;

    }

    public class RoleSelection
    {
        public string RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }


    }
}
