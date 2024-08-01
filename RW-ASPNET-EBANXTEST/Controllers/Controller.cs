using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RW_ASPNET_EBANXTEST.Errors;
using RW_ASPNET_EBANXTEST.Models;
using RW_ASPNET_EBANXTEST.Repository;
using RW_ASPNET_EBANXTEST.Services;
using RW_ASPNET_EBANXTEST.Services.Results;
using System.Text.Json;

namespace RW_ASPNET_EBANXTEST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //
    // Basic main controller created for receive and dispatch events from client.
    // Based on documentation available at ../Docs/Messages.txt
    //
    // In short, we've several accounts, ident by 'account_id', and several 
    // operations (called events) that can be done. These events can be:
    //   - POST /reset, cleaning all cached objects (since persistency is not needed).
    //   - Directly GET /balance with an 'account_id';
    //   - POST 'event' type 'deposit', with an 'destination' account id and an 'amount' value;
    //   - POST 'event' type 'withdraw', with an 'origin' account id and an 'amount' value;
    //   - POST 'event' type 'transfer' with and 'origin' and 'destination' account id and an 'amount' value;
    //
    // Returns:
    //   - FOR /reset, only a 200 OK;
    //   - An '404 0' everytime an inexistent account_id is used;
    //   - An '200 {balance}' when balance is requested;
    //   - An '201' {origin: {id, balance}} everytime an operation with only an origin is successfull.
    //   - An '201' {destination: {id, balance}} everytime an operation with only a destination is successfull.
    //   - An '201' {origin: {id, balance}, destination: {id, balance}} everytime an operation with an origin and a destination is successfull.
    //
    //   - Since there's no restriction about negative balances, and no needs for controlling this, we will not check balance and use signed ints.
    //
    //  Rodrigo Wantuk, Aug-1st-2024.
    //  rodrigowantuk@gmail.com
    //
    public class Controller : ControllerBase
    {

        private readonly ILogger<Controller> _logger;
        private MonetaryOperations bankOperator;

        public Controller(ILogger<Controller> logger)
        {
            _logger = logger;
            bankOperator = new MonetaryOperations(CachedAccountRepository.GetInstance());
            logger.LogInformation("Controller: Created");
        }

        [HttpPost(Name = "Reset")]
        [Route("reset")]
        public ActionResult Reset() // /reset
        {
            _logger.LogInformation("POST: Reset.");
            bankOperator.Reset();
            return Ok();
        }

        [HttpGet(Name = "Balance")]
        [Route("balance")]
        public ActionResult Balance(string account_id) // /balance?account_id=1234
        {
            _logger.LogInformation("GET: Balance for account '" + account_id + "'.");
            try
            {
                return Ok(bankOperator.GetBalance(account_id));
            }
            catch (InvalidAccountException)
            {
                _logger.LogInformation("GET: Balance account not found.");
                return NotFound(0);
            }
        }

        [HttpPost(Name = "Event")]
        [Route("event")]
        public ActionResult Event([FromBody] PostEvent eventData)
        {
            _logger.LogInformation("POST: PostEvent: " + eventData.type);
            try
            {
                return StatusCode(201, bankOperator.ProcessRESTEvent(eventData));
            }
            catch (InvalidAccountException)
            {
                _logger.LogInformation("POST: Account not found.");
                return NotFound(0);
            }
            catch (Exception)
            {
                _logger.LogInformation("POST: Invalid post command rcvd.");
                return NotFound(0);
            }
        }
    }
}