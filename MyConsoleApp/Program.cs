
class Program
{
    static void Main(string[] args)
    {
        System.Console.WriteLine(" --- Press any key to continue --- ");
        
        while (!System.Console.KeyAvailable)
        {
            System.Threading.Thread.Sleep(500);
        }
    }
}
