using System.ComponentModel.DataAnnotations;

namespace SAOV.CLI.AzTools.Enums
{
    public enum MainMenu
    {
        [Display(Name = "Az Cli")]
        AzureCli = 1,

        [Display(Name = "Account")]
        Account = 2,

        [Display(Name = "ResourceGroup")]
        ResourceGroup = 3,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
