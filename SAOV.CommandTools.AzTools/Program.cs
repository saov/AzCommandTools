namespace SAOV.CommandTools.AzTools
{
    using SAOV.CommandTools.AzTools.Views;
    using System.Text;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Welcome.Get();
        }
    }
}
