namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum APIMMenu
    {
        [Display(Name = "Get APIM List")]
        GetAPIMList = 1,

        [Display(Name = "Get APIM List With Operations")]
        GetAPIMListWithOperations = 2,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
