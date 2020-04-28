using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationDemo.Service;
using NotificationDemo.Service.Dto;
using NotificationDemo.Web.Models;

namespace NotificationDemo.Web.Controllers
{
    public class PushApiController : ApiController
    {
        /// <inheritdoc />
        public PushApiController(IPushService pushService,
                                 IUserContextService userContextService)
        {
            _pushService = pushService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// Get VAPID Public Key
        /// </summary>
        /// <returns>VAPID Public Key</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("[action]")]
        public IActionResult GetVapidPublicKey()
        {
            return Ok(_pushService.GetVapidPublicKey());
        }

        /// <summary>
        /// Subscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("[action]")]
        public async Task<ActionResult<SubscriptionDto>> Subscribe([FromBody] PushSubscriptionRequest request)
        {
            var subscription = new SubscriptionDto
            {
                Endpoint = request.Subscription.Endpoint,
                ExpirationTime = request.Subscription.ExpirationTime,
                Auth = request.Subscription.Keys.Auth,
                P256Dh = request.Subscription.Keys.P256Dh
            };

            return await _pushService.Subscribe(subscription, _userContextService.GetCurrentUser().Id);
        }

        /// <summary>
        /// Unsubscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("[action]")]
        public async Task<ActionResult<SubscriptionDto>> Unsubscribe([FromBody] PushSubscriptionRequest request)
        {
            var subscription = new SubscriptionDto
            {
                Endpoint = request.Subscription.Endpoint,
                ExpirationTime = request.Subscription.ExpirationTime,
                Auth = request.Subscription.Keys.Auth,
                P256Dh = request.Subscription.Keys.P256Dh
            };

            await _pushService.Unsubscribe(subscription, _userContextService.GetCurrentUser().Id);

            return subscription;
        }

        /// <summary>
        /// Send a push notifications to a specific user's every device
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="202">Accepted</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessageSelf([FromBody] NotificationDto notification)
        {
            await _pushService.Send(new[] {_userContextService.GetCurrentUser().Id}, notification);

            return Ok();
        }

        /// <summary>
        /// Send a push notifications to a specific user's every device
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="202">Accepted</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage([FromBody] NotificationGroupDto group)
        {
            await _pushService.Send(group.UserIds, group.Notification);

            return Ok();
        }

        private readonly IPushService _pushService;
        private readonly IUserContextService _userContextService;
    }
}
