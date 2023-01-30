using Domain;

namespace Nullam.Contracts.Participant;

public record UpsertParticipantRequest(
    string FirstName,
    string LastName,
    string IdNumber,
    EPaymentType PaymentType,
    string Details);