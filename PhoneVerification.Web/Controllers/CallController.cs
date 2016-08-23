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
            // Find and delete a phone number.
            var number = await _service.FindByPhoneNumberAsync(phoneNumber);
            await _service.DeleteAsync(number);

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
            client.InitiateOutboundCall(new CallOptions
            {
                From = TwilioNumber,                          // The phone number you wish to dial.
                To = phoneNumber,
                Url = "http://www.example.com/call/webhook"   // The URL of call/webhook on your server.
            });

            return Json(new {verificationCode});
        }

        // POST: Call/Webhook
        [HttpPost]
        public async Task<ActionResult> Webhook(string digits, string called)
        {
            var response = new TwilioResponse();

            if (string.IsNullOrEmpty(digits))
            {
                response
                    .Gather(new {numDigits = 6})
                    .Say("Please enter your verification code");
            }
            else
            {
                var number = await _service.FindByPhoneNumberAsync(called);
                if (digits.Equals(number.VerificationCode))
                {
                    response.Say("Thank you! Your phone number has been verified.");
                }
                else
                {
                    response
                        .Gather(new { numDigits = 6 })
                        .Say("Verification code incorrect, please try again.");
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