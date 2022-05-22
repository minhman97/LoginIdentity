using LoginIdentity.Configuration;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace LoginIdentity.Services;

public interface INotifyService
{
    Task SendSms(int phoneNumber, string smsBody);
}

public class NotifyService : INotifyService
{
    private readonly IOptions<TwilioConfiguration> _optionsTwilio;

    public NotifyService(IOptions<TwilioConfiguration> optionsTwilio)
    {
        _optionsTwilio = optionsTwilio;
    }

    public async Task SendSms(int phoneNumber, string smsBody)
    {
        var accountSid = _optionsTwilio.Value.AccountSid;
        var authToken = _optionsTwilio.Value.AuthToken;
        TwilioClient.Init(accountSid, authToken);

        var message = await MessageResource.CreateAsync(
            body: smsBody,
            from: new PhoneNumber("+" + _optionsTwilio.Value.PhoneNumber),
            to: new PhoneNumber("+84" + phoneNumber)
        );
    }
}