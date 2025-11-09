using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using OldPhonePad.Lib;

namespace OldPhonePad.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Old Phone Pad Decoder");
            Console.WriteLine("Enter keypresses ending with '#' (or press Enter to run sample inputs):");

            var services = new ServiceCollection();
            services.AddSingleton<IKeyMapping, DefaultKeyMapping>();
            services.AddSingleton<IInputTokenizer, SimpleTokenizer>();
            services.AddSingleton<IGroupResolver, GroupResolver>();
            services.AddSingleton<IOldPhonePadService, OldPhonePadService>();

            using var provider = services.BuildServiceProvider();
            var svc = provider.GetRequiredService<IOldPhonePadService>();

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                // Run some demo samples
                var samples = new[]
                {
                    "33#",
                    "227*#",
                    "4433555 555666#",
                    "8 88777444666*664#"
                };

                foreach (var s in samples)
                {
                    var output = svc.Decode(s);
                    Console.WriteLine($"{s} → {output}");
                }

                return;
            }

            try
            {
                var result = svc.Decode(input);
                Console.WriteLine($"Decoded: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
