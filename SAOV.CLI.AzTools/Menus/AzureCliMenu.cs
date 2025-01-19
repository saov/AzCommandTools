namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum AzureCliMenu
    {
        [Display(Name = "Az Cli Upgrade")]
        AzCliUpgrade = 1,

        [Display(Name = "Get Extensions Installed List")]
        GetExtensionsInstalledList = 2,

        [Display(Name = "Ge tExtensions Available List")]
        GetExtensionsAvailableList = 3,

        [Display(Name = "Update Extension")]
        UpdateExtension = 4,

        [Display(Name = "Add Extension")]
        AddExtension = 5,

        [Display(Name = "Remove Extension")]
        RemoveExtension = 6,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
