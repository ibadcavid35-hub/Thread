namespace ConsoleApp44
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text = new string('A', 100000);
            string from = "from.txt";
            string to = "to.txt";
            
            Copy.CopyOperation(from,to,text);
            
        }
    }
}
