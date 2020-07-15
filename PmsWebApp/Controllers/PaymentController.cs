using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using PayFast;
using PayFast.AspNetCore;

namespace PmsWebApp.Controllers
{
    public class PaymentController : Controller
    {

        #region Fields

        private readonly PayFastSettings payFastSettings;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public PaymentController(IOptions<PayFastSettings> payFastSettings, ILogger<PaymentController> logger)
        {
            this.payFastSettings = payFastSettings.Value;
            this.logger = logger;
        }

        #endregion Constructor

        //#region Methods

        public IActionResult Index(int AmountDue)
        {
        

            return View(AmountDue);
        }

        public IActionResult OnceOff( int AmountDue)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl; 
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details

            onceOffRequest.email_address = "u11166933@tuks.co.za";//"sbtu01@payfast.co.za";
            // Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = AmountDue; 
            onceOffRequest.item_name = "Rental payment for the month";
            onceOffRequest.item_description = "Some details about the once off payment";

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "u11166933@tuks.co.za"; //"sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Notify([ModelBinder(BinderType = typeof(PayFastNotifyModelBinder))]PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            this.logger.LogInformation($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, this.HttpContext.Connection.RemoteIpAddress);

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            this.logger.LogInformation($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = await payfastValidator.ValidateSourceIp();

            this.logger.LogInformation($"Ip Address Validation Result: {ipAddressValidationResult}");

            // Currently seems that the data validation only works for success
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                this.logger.LogInformation($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                this.logger.LogInformation($"Subscription was cancelled");
            }

            return Ok();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Return()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

    }
}