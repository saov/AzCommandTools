namespace SAOV.CommandTools.AzTools.Views
{
    using System.ComponentModel.DataAnnotations;

    public enum MenuOptions
    {
        [Display(Name = "Az Cli Version")]
        AzCliVersion = 1,

        [Display(Name = "Az Account Show")]
        AzAccountShow = 2,

        [Display(Name = "Az Account Subscription List")]
        AzAccountSubscriptionList = 3,

        [Display(Name = "AzResourceGroupList")]
        AzResourceGroupList = 4,

        [Display(Name = "About")]
        About = 5,

        [Display(Name = "Exit")]
        Exit = 6,
    }
}
