using System.ComponentModel.DataAnnotations;

namespace SAOV.CLI.AzTools.Menus
{
    public enum AccountMenu
    {
        [Display(Name = "LogIn")]
        LogIn = 1,

        [Display(Name = "LogOut")]
        LogOut = 2,

        [Display(Name = "Show Current Subscription")]
        ShowCurrentSubscription = 3,

        [Display(Name = "Get Subscription List")]
        GetSubscriptionList = 4,

        [Display(Name = "Set Subscription")]
        SetSubscription = 5,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
