namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum VnetMenu
    {
        [Display(Name = "Get Vnet List")]
        GetVnetList = 1,

        [Display(Name = "Get Vnets With Subnets")]
        GetVnetListWithSubnets = 2,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
