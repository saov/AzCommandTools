namespace SAOV.CommandTools.AzTools.Views
{
    using System.ComponentModel.DataAnnotations;

    public enum MenuOptionsEnum
    {
        [Display(Name = "Azure Login")]
        AzLogin = 1,

        [Display(Name = "Cli Version Installed")]
        AzCliVersion = 2,

        [Display(Name = "Show Account Info")]
        AzAccountShow = 3,

        [Display(Name = "Get Subscriptions")]
        AzAccountSubscriptionList = 4,

        [Display(Name = "Get ResourceGroups")]
        AzResourceGroupList = 5,

        [Display(Name = "Get KeyVaults")]
        AzKeyVaultList = 6,

        [Display(Name = "About SAOV Azure Tools...")]
        About = 7,

        [Display(Name = "Azure LogOut")]
        AzLogOut = 8,

        [Display(Name = "<< Exit >>")]
        Exit = 9,
    }
}
