namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Components;
    using Spectre.Console;

    internal static class NavigationMap
    {
        internal static bool GetNavigationMap()
        {
            AnsiConsole.Clear();
            ModuleHeader.Show("/NavigationMap");
            Tree menu = new Tree(new Markup("[Magenta1]Menu[/]"))
                            .Style("40")
                            .Guide(TreeGuide.DoubleLine);
            TreeNode menu_Docker = menu.AddNode(new Tree("[Turquoise2]Docker[/]"));
            _ = menu_Docker.AddNode(new Tree("[Gold1]RemoveAllUnusedContainersNetworksImagesAndVolumes[/] [red]-->[/] Remove All Unused Containers, Networks, Images And Volumes"));
            TreeNode menu_Kubernetes = menu.AddNode(new Tree("[Turquoise2]Kubernetes[/]"));
            _ = menu_Kubernetes.AddNode(new Tree("[Gold1]AssignContext[/] [red]-->[/] Assign Context"));
            _ = menu_Kubernetes.AddNode(new Tree("[Gold1]SetNameSpaces[/] [red]-->[/] Set NameSpace"));
            _ = menu_Kubernetes.AddNode(new Tree("[Gold1]DecodeSecret[/] [red]-->[/] Decode Secret"));
            _ = menu_Kubernetes.AddNode(new Tree("[Gold1]DeletePodsNotRunning[/] [red]-->[/] Delete Pods NotRunning"));
            TreeNode menu_AzureCli = menu.AddNode(new Tree("[Turquoise2]AzureCli[/]"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]AzCliUpgrade[/] [red]-->[/] Az Cli Upgrade"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]GetExtensionsInstalledList[/] [red]-->[/] Get Extensions Installed List"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]GetExtensionsAvailableList[/] [red]-->[/] Get Extensions Available List"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]UpdateExtension[/] [red]-->[/] Update Extension"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]AddExtension[/] [red]-->[/] Add Extension"));
            _ = menu_AzureCli.AddNode(new Tree("[Gold1]RemoveExtension[/] [red]-->[/] Remove Extension"));
            TreeNode menu_Account = menu.AddNode(new Tree("[Turquoise2]Account[/]"));
            _ = menu_Account.AddNode(new Tree("[Gold1]LogIn[/] [red]-->[/] Log In"));
            _ = menu_Account.AddNode(new Tree("[Gold1]LogOut[/] [red]-->[/] Log Out"));
            _ = menu_Account.AddNode(new Tree("[Gold1]ShowCurrentSubscription[/] [red]-->[/] Show Current Subscription"));
            _ = menu_Account.AddNode(new Tree("[Gold1]GetSubscriptionList[/] [red]-->[/] Get Subscription List"));
            _ = menu_Account.AddNode(new Tree("[Gold1]SetSubscription[/] [red]-->[/] Set Subscription"));
            TreeNode menu_ACR = menu.AddNode(new Tree("[Turquoise2]ACR[/]"));
            _ = menu_ACR.AddNode(new Tree("[Gold1]GetACRList[/] [red]-->[/] Get ACR List"));
            _ = menu_ACR.AddNode(new Tree("[Gold1]GetACRRepositories[/] [red]-->[/] Get ACR Repositories"));
            _ = menu_ACR.AddNode(new Tree("[Gold1]GetACRRepositoryTags[/] [red]-->[/] Get ACR Repository Tags"));
            TreeNode menu_APIM = menu.AddNode(new Tree("[Turquoise2]APIM[/]"));
            _ = menu_APIM.AddNode(new Tree("[Gold1]GetAPIMList[/] [red]-->[/] Get APIM List"));
            _ = menu_APIM.AddNode(new Tree("[Gold1]GetAPIMListWithOperations[/] [red]-->[/] Get APIM List With Operations"));
            TreeNode menu_KeyVault = menu.AddNode(new Tree("[Turquoise2]KeyVault[/]"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaultList[/] [red]-->[/] Get KeyVault List"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaultListWithNetworkRules[/] [red]-->[/] Get KeyVault List With Network Rules"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]GetKeyVaulSecretList[/] [red]-->[/] Get KeyVaul Secret List"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]KeyVaultSecretShow[/] [red]-->[/] Key Vault Secret Show"));
            _ = menu_KeyVault.AddNode(new Tree("[Gold1]KeyVaultAllSecretShow[/] [red]-->[/] Key Vault All Secret Show"));
            TreeNode menu_ResourceGroup = menu.AddNode(new Tree("[Turquoise2]ResourceGroup[/]"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourceGroupList[/] [red]-->[/] Get ResourceGroup List"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourcesInResouceGroup[/] [red]-->[/] Get Resources In ResouceGroup"));
            _ = menu_ResourceGroup.AddNode(new Tree("[Gold1]GetResourcesInSubscription[/] [red]-->[/] Get Resources In Subscription"));
            TreeNode menu_Vnet = menu.AddNode(new Tree("[Turquoise2]Vnet[/]"));
            _ = menu_Vnet.AddNode(new Tree("[Gold1]GetVnetList[/] [red]-->[/] Get Vnet List"));
            _ = menu_Vnet.AddNode(new Tree("[Gold1]GetVnetListWithSubnets[/] [red]-->[/] Get Vnets With Subnets"));
            _ = menu.AddNode(new Tree("[Turquoise2]QueryFilters[/] [red]-->[/] Resource Search Filter"));
            _ = menu.AddNode(new Tree("[Turquoise2]NavigationMap[/] [red]-->[/] Navigation Map"));
            _ = menu.AddNode(new Tree("[Turquoise2]Exit[/] [red]-->[/] Exit"));
            AnsiConsole.Write(menu);
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Markup("[green]Press any key to back.[/]"));
            _ = Console.ReadKey();
            return true;
        }
    }
}
