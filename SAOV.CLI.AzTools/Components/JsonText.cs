namespace SAOV.CLI.AzTools.Components
{
    using SAOV.CLI.AzTools.Helpers;
    using Spectre.Console;
    using Spectre.Console.Json;

    internal static class JsonText
    {
        internal static Spectre.Console.Json.JsonText Show<T>(T dataRaw)
        {
            return new Spectre.Console.Json.JsonText(JsonHelper.GetJson<T>(dataRaw))
                            .BracesColor(Color.Red)
                            .BracketColor(Color.Magenta1)
                            .ColonColor(Color.Gold1)
                            .CommaColor(Color.Gold1)
                            .MemberColor(172)
                            .StringColor(Color.Green)
                            .NumberColor(Color.Blue)
                            .BooleanColor(Color.Turquoise2)
                            .NullColor(Color.Magenta1);
        }
    }
}
