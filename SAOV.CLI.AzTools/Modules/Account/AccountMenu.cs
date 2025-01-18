using System.ComponentModel.DataAnnotations;

namespace SAOV.CLI.AzTools.Modules.Account
{
    public enum AccountMenu
    {
        [Display(Name = "LogIn By Service Principal")]
        LogInByServicePrincipal = 1,

        [Display(Name = "LogOut")]
        LogOut = 2,

        [Display(Name = "<< Exit >>")]
        Exit = 99,
    }
}
