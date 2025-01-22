namespace SAOV.CLI.AzTools.Menus
{
    using System.ComponentModel.DataAnnotations;

    internal enum KeyVaultMenu
    {
        [Display(Name = "Get KeyVault List")]
        GetKeyVaultList = 1,

        [Display(Name = "Get KeyVault List With Network Rules")]
        GetKeyVaultListWithNetworkRules = 2,

        [Display(Name = "Get KeyVaul Secret List")]
        GetKeyVaulSecretList = 3,

        [Display(Name = "Key Vault Secret Show")]
        KeyVaultSecretShow = 4,

        [Display(Name = "Key Vault All Secret Show")]
        KeyVaultAllSecretShow = 5,

        [Display(Name = "Exit")]
        Exit = 99,
    }
}
