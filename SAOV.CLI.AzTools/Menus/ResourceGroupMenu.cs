namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum ResourceGroupMenu
    {
        [Display(Name = "Get ResourceGroup List")]
        GetResourceGroupList = 1,

        [Display(Name = "Get Resources In ResouceGroup")]
        GetResourcesInResouceGroup = 2,

        [Display(Name = "Get Resources In Subscription")]
        GetResourcesInSubscription = 3,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
