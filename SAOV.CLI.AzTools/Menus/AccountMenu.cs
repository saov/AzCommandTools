namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum AccountMenu
    {
        [Display(Name = "Log In")]
        LogIn = 1,

        [Display(Name = "Log Out")]
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
