namespace SAOV.CLI.AzTools.Components
{
    using Spectre.Console;

    internal static class TextPrompt
    {
        internal static bool Show(string question, bool defaultValue = false)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<bool>(question)
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(defaultValue)
                    .WithConverter(choice => choice ? "y" : "n"));
        }

        internal static T Show<T>(string question)
        {
            return AnsiConsole.Ask<T>(question);
        }

        internal static T Show<T>(string question, List<T> choises, int indexDefaultValue)
        {
            if (indexDefaultValue >= choises.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return AnsiConsole.Prompt(
                new TextPrompt<T>(question)
                    .AddChoices([.. choises])
                    .DefaultValue(choises[indexDefaultValue]));
        }

        internal static string Show(string question, Func<string, bool> validationRules, string errorMessage, bool cancelOption = true, bool isSecret = false)
        {
            TextPrompt<string> textPrompt = new TextPrompt<string>(question)
                                            .Validate((value) =>
                                                {
                                                    return validationRules(value) ?
                                                        ValidationResult.Success() :
                                                        ValidationResult.Error($"[red]{errorMessage}[/]");
                                                }
                                            );
            if (cancelOption)
            {
                textPrompt.DefaultValue(string.Empty)
                          .HideDefaultValue();
            }
            if (isSecret)
            {
                textPrompt.Secret();
            }
            return AnsiConsole.Prompt(textPrompt);
        }

    }
}
