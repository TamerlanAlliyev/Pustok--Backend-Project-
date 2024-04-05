using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Account
{
    public class AccountLoginVM
    {
        public string UsernameOrEmail { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
