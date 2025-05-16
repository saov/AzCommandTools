namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum ACRMenu
    {
        [Display(Name = "Get ACR List")]
        GetACRList = 1,

        [Display(Name = "Get ACR Repositories")]
        GetACRRepositories = 2,

        [Display(Name = "Get ACR Repository Tags")]
        GetACRRepositoryTags = 3,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
