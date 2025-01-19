using System.ComponentModel.DataAnnotations;

namespace SAOV.CLI.AzTools.Menus
{
    internal enum ResourceGroupMenu
    {
        [Display(Name = "Get ResourceGroup List")]
        GetResourceGroupList = 1,

        [Display(Name = "Get Resources In ResouceGroup")]
        GetResourcesInResouceGroup = 2,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
