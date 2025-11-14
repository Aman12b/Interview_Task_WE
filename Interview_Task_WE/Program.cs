using System;
using Application.UseCases;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessTariffSwitchRequestsUseCase useCase = CompositionRoot.BuildUseCase();
                useCase.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error: " + ex.Message);
            }

            Console.WriteLine("Finished. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
