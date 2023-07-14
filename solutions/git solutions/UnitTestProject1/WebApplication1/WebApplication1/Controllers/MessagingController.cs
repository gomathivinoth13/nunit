using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace WebApplication1.Controllers
{
    public class MessagingController : TwilioController
    {

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public TwiMLResult Index(SmsRequest request)
        {
            var response = new MessagingResponse();
            response.Message("Hello World");
            return TwiML(response);
        }
    }
}