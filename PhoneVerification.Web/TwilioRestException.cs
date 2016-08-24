using System;
using Twilio;

namespace PhoneVerification.Web
{
    public class TwilioRestException : Exception
    {
        public TwilioRestException(RestException restException)
            : base($"{restException.Code}: {restException.Message}")
        {
        }
    }
}