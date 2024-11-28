namespace SAOV.CommandTools.AzTools.Views
{
    using System.ComponentModel.DataAnnotations;

    public enum MenuOptionsEnum
    {
        [Display(Name = "Cli Version Installed")]
        AzCliVersion = 1,

        [Display(Name = "Show Account Info")]
        AzAccountShow = 2,

        [Display(Name = "Get Subscriptions")]
        AzAccountSubscriptionList = 3,

        [Display(Name = "Get ResourceGroups")]
        AzResourceGroupList = 4,

        [Display(Name = "About SAOV Azure Tools...")]
        About = 5,

        [Display(Name = "<< Exit >>")]
        Exit = 6,
    }
}
