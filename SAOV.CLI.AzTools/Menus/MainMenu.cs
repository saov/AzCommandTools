namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum MainMenu
    {
        [Display(Name = "Azure Query Filter In Commands")]
        AzureQueryFilterInCommands = 1,

        [Display(Name = "Az Cli")]
        AzureCli = 2,

        [Display(Name = "Account")]
        Account = 3,

        [Display(Name = "ResourceGroup")]
        ResourceGroup = 4,

        [Display(Name = "KeyVault")]
        KeyVault = 5,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
