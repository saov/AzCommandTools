namespace SAOV.CLI.AzTools
{
    using SAOV.CLI.AzTools.Helpers;
    using SAOV.CLI.AzTools.Menus;
    using SAOV.CLI.AzTools.Modules.Account;
    using SAOV.CLI.AzTools.Modules.AzureCli;
    using SAOV.CLI.AzTools.Modules.AzureCli.Entities;
    using SAOV.CLI.AzTools.Modules.ResourceGroup;
    using Spectre.Console;
    using System.Text;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //DemoComponents();

            AzCliVersionEntity azVersionEntity = CommandHelper.Run<AzCliVersionEntity>(AzCommands.AzureCli_Version, []);
            if (azVersionEntity != null)
            {
                List<string> choices = [];
                bool run = true;
                Enum.GetValues<MainMenu>().ToList().ForEach(item => { choices.Add(item.ToString()); });
                while (run)
                {
                    AnsiConsole.Clear();
                    Banner.Show();
                    string selectionPromptValue = Components.SelectionPrompt.Show(choices);
                    _ = Enum.TryParse(selectionPromptValue, out MainMenu mainMenuOption);
                    run = mainMenuOption switch
                    {
                        MainMenu.AzureCli => AzureCli.Show(),
                        MainMenu.Account => Account.Show(),
                        MainMenu.ResourceGroup => ResourceGroup.Show(),
                        MainMenu.Exit => false,
                        _ => false
                    };
                }
                Console.Clear();
            }
        }

        #region Demo DemoComponents

        static void DemoComponents()
        {
            int status = Components.Status.Show<int>("Espere...", () => { Thread.Sleep(3000); return 50; });
            AnsiConsole.WriteLine(status);
            string selectionPromptValue = Components.SelectionPrompt.Show(["op1", "op2", "op3", "op4", "op5", "op6", "op7", "op8", "op9", "op10", "op11", "op12"]);
            AnsiConsole.WriteLine(selectionPromptValue);
            bool textPromptValue01 = Components.TextPrompt.Show("Desea continuar?");
            AnsiConsole.WriteLine(textPromptValue01);
            string textPromptValue02 = Components.TextPrompt.Show<string>("¿Cual es tu nombre?");
            AnsiConsole.WriteLine(textPromptValue02);
            string textPromptValue03 = Components.TextPrompt.Show<string>("Selecciona una opcion", ["Uno", "Dos", "Tres"], 1);
            AnsiConsole.WriteLine(textPromptValue03);
            int textPromptValue04 = Components.TextPrompt.Show<int>("Selecciona una opcion", [1, 2, 3], 1);
            AnsiConsole.WriteLine(textPromptValue04);
            string textPromptValue05 = Components.TextPrompt.Show("Proporcione la contraseña : ", (value) => { return value == "123"; }, "Error!!!", true, true);
            AnsiConsole.WriteLine(textPromptValue05);
            List<KeyValuePair<Markup, Justify>> columns =
            [
                new(new("[red]Columna01[/]"), Justify.Left),
                new(new("[red]Columna02[/]"), Justify.Center),
                new(new("[red]Columna03[/]"), Justify.Right),
                new(new("[red]Columna04[/]"), Justify.Center),
            ];
            List<MyClass> myEntities = GetEntities();
            List<List<Markup>> rows = [];
            myEntities.ForEach(item =>
            {
                rows.Add([new($"[93]{item.MyProperty01}[/]"),
                          new($"[yellow]{item.MyProperty02}[/]"),
                          new($"[purple]{item.MyProperty03}[/]"),
                          new($"[40]{item.MyProperty04}[/]")]);
            });
            AnsiConsole.Write(Components.Table.Show(true, "My Title", "My Caption", columns, rows));
            AnsiConsole.WriteLine();
        }

        static List<MyClass> GetEntities()
        {
            List<MyClass> myEntity = [];
            for (int i = 0; i < 4; i++)
            {
                myEntity.Add(new MyClass()
                {
                    MyProperty01 = $"0{i + 1}-01",
                    MyProperty02 = $"0{i + 1}-02",
                    MyProperty03 = $"0{i + 1}-03",
                    MyProperty04 = $"0{i + 1}-04",
                });
            }
            return myEntity;
        }

        class MyClass
        {
            public string MyProperty01 { get; set; }
            public string MyProperty02 { get; set; }
            public string MyProperty03 { get; set; }
            public string MyProperty04 { get; set; }
        };

        #endregion Demo DemoComponents
    }
}
