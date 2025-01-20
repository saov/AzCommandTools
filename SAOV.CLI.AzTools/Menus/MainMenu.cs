namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum MainMenu
    {
        [Display(Name = "Az Cli")]
        AzureCli = 1,

        [Display(Name = "Account")]
        Account = 2,

        [Display(Name = "ResourceGroup")]
        ResourceGroup = 3,

        [Display(Name = "KeyVault")]
        KeyVault = 4,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
