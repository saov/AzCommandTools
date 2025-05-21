namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum MainMenu
    {
        [Display(Name = "Docker")]
        Docker = 1,

        [Display(Name = "Kubernetes")]
        Kubernetes = 2,

        [Display(Name = "Az Cli")]
        AzureCli = 3,

        [Display(Name = "Account")]
        Account = 4,

        [Display(Name = "ACR")]
        ACR = 5,

        [Display(Name = "APIM")]
        APIM = 6,

        [Display(Name = "KeyVault")]
        KeyVault = 7,

        [Display(Name = "ResourceGroup")]
        ResourceGroup = 8,

        [Display(Name = "Vnet")]
        Vnet = 9,

        [Display(Name = "Query Filters")]
        QueryFilters = 97,

        [Display(Name = "Navigation Map")]
        NavigationMap = 98,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
