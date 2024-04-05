using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Account
{
    public class ResetPasswordVM
    {
        public string user { get; set; } = null!;
        public string token { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
