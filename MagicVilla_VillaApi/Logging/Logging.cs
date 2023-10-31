namespace DreamVilla_VillaApi.Logging
{
    public class Logging : ILogging
    {
        void ILogging.Logging(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine($"Error - {message}");
            }
            Console.WriteLine($"{message}");
        }
    }
}
