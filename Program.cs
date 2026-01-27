namespace VardagshörnanApp
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            await Common.CustomerOrAdminAsync();
        }
    }
}
