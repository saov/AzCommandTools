namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum DockerMenu
    {
        [Display(Name = "Remove All Unused Containers, Networks, Images And Volumes")]
        RemoveAllUnusedContainersNetworksImagesAndVolumes = 1,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
