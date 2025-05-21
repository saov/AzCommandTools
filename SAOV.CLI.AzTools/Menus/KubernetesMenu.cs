namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    public enum KubernetesMenu
    {
        [Display(Name = "Assign Context")]
        AssignContext = 1,

        [Display(Name = "Set NameSpace")]
        SetNameSpaces = 2,

        [Display(Name = "Decode Secret")]
        DecodeSecret = 3,

        [Display(Name = "Delete Pods NotRunning")]
        DeletePodsNotRunning = 4,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
