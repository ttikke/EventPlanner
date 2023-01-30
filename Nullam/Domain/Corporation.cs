using ErrorOr;
using ServiceErrors;

namespace Domain;

public class Corporation
{

    public const int MaxDetailsLength = 5000;
    public const int MinNameLength = 1;
    
    public Guid Id { get; set; }
    public string Name { get; private set; } = "";
    public string RegistrationCode { get; private set;} = "";
    public int NumberOfParticipants { get; private set;}
    public EPaymentType PaymentType { get; private set;}
    public string Details { get; private set;} = "";

    private Corporation() {}

    private Corporation(Guid id, string name, string registrationCode, int numberOfParticipants, EPaymentType paymentType,
        string details)
    {
        Id = id;
        Name = name;
        RegistrationCode = registrationCode;
        NumberOfParticipants = numberOfParticipants;
        PaymentType = paymentType;
        Details = details;
    }
    
    public static ErrorOr<Corporation> Create(string name, string registrationCode, int numberOfParticipants,
        EPaymentType paymentType, string details, Guid? id = null)
    {
        List<Error> errors = new List<Error>();
        if (name.Length < MinNameLength)
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

        return new Corporation(
            id ?? Guid.NewGuid(),
            name,
            registrationCode,
            numberOfParticipants,
            paymentType,
            details
        );

    }
    
}