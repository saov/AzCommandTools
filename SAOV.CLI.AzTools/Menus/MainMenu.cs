namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum MainMenu
    {
        [Display(Name = "Az Cli")]
        AzureCli = 1,

        [Display(Name = "Account")]
        Account = 2,

        [Display(Name = "ACR")]
        ACR = 3,

        [Display(Name = "APIM")]
        APIM = 4,

        [Display(Name = "KeyVault")]
        KeyVault = 5,

        [Display(Name = "ResourceGroup")]
        ResourceGroup = 6,

        [Display(Name = "Vnet")]
        Vnet = 7,

        [Display(Name = "Query Filters")]
        QueryFilters = 97,

        [Display(Name = "Navigation Map")]
        NavigationMap = 98,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
