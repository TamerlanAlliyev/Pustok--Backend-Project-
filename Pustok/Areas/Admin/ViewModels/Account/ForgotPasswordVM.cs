using System.ComponentModel.DataAnnotations;

namespace Pustok.Areas.Admin.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
