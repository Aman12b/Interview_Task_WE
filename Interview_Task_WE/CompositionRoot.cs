using Application.Ports;
using Application.Services;
using Application.UseCases;
using Infrastructure.Csv;
using Infrastructure.Presentation;
using Infrastructure.Stores;
using Infrastructure.Time;

namespace ConsoleApp
{
    public static class CompositionRoot
    {
        public static ProcessTariffSwitchRequestsUseCase BuildUseCase()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string inputDir = Path.Combine(basePath, "InputFiles");

            string customersPath = Path.Combine(inputDir, "customers.csv");
            string tariffsPath = Path.Combine(inputDir, "tariffs.csv");
            string requestsPath = Path.Combine(inputDir, "requests.csv");
            string ledgerPath = Path.Combine(inputDir, "processed.txt");
            string followupsPath = Path.Combine(inputDir, "followups.csv");

            ICustomerRepository customers = new CustomerCsvRepository(customersPath);
            ITariffRepository tariffs = new TariffCsvRepository(tariffsPath);
            IRequestReader requests = new RequestCsvRepository(requestsPath);
            IProcessedLedgerStore ledger = new ProcessedLedgerStore(ledgerPath);
            IFollowUpActionStore followUps = new FollowUpActionCsvStore(followupsPath);
            IOutput output = new ConsolePresenter();
            ITimeProvider timeProvider = new SystemTimeProviderVienna();

            var slaOptions = new SlaOptions
            {
                StandardHours = 48,
                PremiumHours = 24,
                SmartMeterUpgradeExtraHours = 12
            };

            var slaService = new SlaService(timeProvider, slaOptions);
            var idemService = new IdempotencyService(ledger);

            var useCase = new ProcessTariffSwitchRequestsUseCase(
                customers,
                tariffs,
                requests,
                ledger,
                followUps,
                output,
                slaService,
                idemService
            );

            return useCase;
        }
    }
}
