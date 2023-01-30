using ErrorOr;
using ServiceErrors;

namespace Domain;

public class Participant
{
    public const int MaxDetailsLength = 1500;
    public const int MinNameLength = 1;
    
    public Guid Id { get; set; }
    public string FirstName { get; private set; } = "";
    public string LastName { get; private set; } = "";
    public string IdNumber { get; private set; } = "";
    public EPaymentType PaymentType { get; private set; }
    public string Details { get; private set; } = "";

    private Participant() {}

    private Participant(Guid id, string firstName, string lastName, string idNumber, EPaymentType paymentType, string details)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        IdNumber = idNumber;
        PaymentType = paymentType;
        Details = details;
    }

    public static ErrorOr<Participant> Create(string firstName, string lastName, string idNumber,
        EPaymentType paymentType, string details, Guid? id = null)
    {
        List<Error> errors = new List<Error>();
        if (firstName.Length < MinNameLength || lastName.Length < MinNameLength)
        {
            errors.Add( Errors.Participant.NameTooShort(MinNameLength));
        }

        if (details.Length > MaxDetailsLength)
        {
            errors.Add(Errors.Participant.DetailsTooLong(MaxDetailsLength));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Participant(
            id ?? Guid.NewGuid(),
            firstName,
            lastName,
            idNumber,
            paymentType,
            details
        );

    }

}