using System.Collections.Generic;
using Application.Ports;
using Application.Services;
using DomainModel;

namespace Application.UseCases
{
    public class ProcessTariffSwitchRequestsUseCase
    {
        private readonly ICustomerRepository _customers;
        private readonly ITariffRepository _tariffs;
        private readonly IRequestReader _requests;
        private readonly IProcessedLedgerStore _ledger;
        private readonly IFollowUpActionStore _followUps;
        private readonly IOutput _output;
        private readonly SlaService _slaService;
        private readonly IdempotencyService _idemService;

        public ProcessTariffSwitchRequestsUseCase(
            ICustomerRepository customers,
            ITariffRepository tariffs,
            IRequestReader requests,
            IProcessedLedgerStore ledger,
            IFollowUpActionStore followUps,
            IOutput output,
            SlaService slaService,
            IdempotencyService idemService)
        {
            _customers = customers;
            _tariffs = tariffs;
            _requests = requests;
            _ledger = ledger;
            _followUps = followUps;
            _output = output;
            _slaService = slaService;
            _idemService = idemService;
        }

        public void Execute()
        {
            int total = 0;
            int approved = 0;
            int rejected = 0;
            int skipped = 0;
            int skippedAlreadyProcessed = 0;

            IEnumerable<Request> newRequests = _requests.ReadNew();

            foreach (Request req in newRequests)
            {
                total++;

                if (!_idemService.ShouldProcess(req.RequestId))
                {
                    skippedAlreadyProcessed++;
                    continue;
                }

                Customer customer = _customers.GetById(req.CustomerId);
                Tariff tariff = _tariffs.GetById(req.TargetTariffId);

                EvaluationResult evaluationResult = req.Decide(customer, tariff);
                bool hasUpgradeFollowUp = evaluationResult.FollowUps != null && evaluationResult.FollowUps.Count > 0;

                // SLA berechnen
                System.DateTimeOffset due = _slaService.CalculateDue(customer, req, hasUpgradeFollowUp);

                // endgültige Decision mit SlaDueAt
                Decision finalDecision = evaluationResult.Decision.WithDue(due);

                if (evaluationResult.FollowUps != null && evaluationResult.FollowUps.Count > 0)
                {
                    // Due auf FollowUps setzen
                    foreach (FollowUpAction action in evaluationResult.FollowUps)
                    {
                        action.SetDue(due);
                    }
                    _followUps.SaveMany(evaluationResult.FollowUps);
                }

                // Idempotenz
                _idemService.MarkProcessed(req.RequestId);

                // Zählen
                if (finalDecision.Kind == DecisionKind.Approved) approved++;
                else if (finalDecision.Kind == DecisionKind.Rejected) rejected++;
                else skipped++;

                _output.WriteResult(req, finalDecision, evaluationResult.FollowUps);
            }

            _output.WriteSummary(total, approved, rejected, skipped, skippedAlreadyProcessed);
        }
    }
}
