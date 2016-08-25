using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using PhoneVerification.Web.Models;
using PhoneVerification.Web.Services;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;
using Number = PhoneVerification.Web.Models.Number;

namespace PhoneVerification.Web.Controllers
{
    public class CallController : TwilioController
    {
        private readonly INumbersService _service;

        public static string BaseUrl => WebConfigurationManager.AppSettings["BaseUrl"];
        public static string TwilioAccountSID => WebConfigurationManager.AppSettings["TwilioAccountSID"];
        public static string TwilioAuthToken => WebConfigurationManager.AppSettings["TwilioAuthToken"];
        public static string TwilioNumber => WebConfigurationManager.AppSettings["TwilioNumber"];

        public CallController()
        {
            _service = new NumbersService(new PhoneVerificationDbContext());
        }

        // POST: Call/Create
        [HttpPost]
        public async Task<JsonResult> Create(string phoneNumber)
        {
            // Delete a phone number.
            await _service.DeleteByPhoneNumberAsync(phoneNumber);

            // Create a phone number.
            var verificationCode = GenerateVerificationCode();
            await _service.CreateAsync(
                new Number
                {
                    PhoneNumber = phoneNumber,
                    VerificationCode = verificationCode,
                    Verified = false
                });

            // Make the phone call.
            var client = new TwilioRestClient(TwilioAccountSID, TwilioAuthToken);
            var call = client.InitiateOutboundCall(new CallOptions
            {
                From = TwilioNumber,               // The phone number you wish to dial.
                To = phoneNumber,
                Url = $"{BaseUrl}/call/twiml"      // The URL of call/twiml on your server.
            });

            if (call.RestException != null)
            {
                // If there is an exception, fail loudly.
                throw new TwilioRestException(call.RestException);
            }

            return Json(new {verificationCode});
        }

        // POST: Call/Twiml
        [HttpPost]
        public async Task<ActionResult> Twiml(string digits, string called)
        {
            var response = new TwilioResponse();

            if (string.IsNullOrEmpty(digits))
            {
                response
                    .BeginGather(new {numDigits = 6, timeOut = 10})
                    .Say("Please enter your verification.")
                    .EndGather();
            }
            else
            {
                var number = await _service.FindByPhoneNumberAsync(called);
                if (digits.Equals(number.VerificationCode))
                {
                    number.Verified = true;
                    await _service.UpdateAsync(number);

                    response.Say("Thank you! Your phone number has been verified.");
                }
                else
                {
                    response
                        .BeginGather(new {numDigits = 6, timeOut = 10})
                        .Say("Verification code incorrect, please try again.")
                        .EndGather();
                }
            }

            return TwiML(response);
        }

        // POST: Call/Status
        [HttpPost]
        public async Task<JsonResult> Status(string phoneNumber)
        {
            var number = await _service.FindByPhoneNumberAsync(phoneNumber);
            var status = number.Verified ? "verified" : "unverified";

            return Json(new {status});
        }

        private static string GenerateVerificationCode()
        {
            var code = new Random().Next(100000, 999999);
            return code.ToString();
        }
    }
}