using System.ComponentModel.DataAnnotations;

namespace SAOV.CLI.AzTools.Enums
{
    public enum MainMenu
    {
        [Display(Name = "Account")]
        Account = 1,

        [Display(Name = "<< Exit >>")]
        Exit = 99,
    }
}
