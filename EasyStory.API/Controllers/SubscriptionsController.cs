﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyStory.API.Domain.Models;
using EasyStory.API.Domain.Services;
using EasyStory.API.Extensions;
using EasyStory.API.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EasyStory.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SubscriptionsController(ISubscriptionService subscriptionService, IUserService userService ,IMapper mapper)
        {
            _userService = userService;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        [SwaggerOperation(
            Summary = "List all Subscriptions",
            Description = "List of Subscriptions",
            OperationId = "ListAllSubscriptions",
            Tags = new[] { "Subscriptions" }
        )]
        [SwaggerResponse(200, "List of Subscriptions", typeof(IEnumerable<SubscriptionResource>))]
        [HttpGet("subscriptions")]
        public async Task<IEnumerable<SubscriptionResource>> GetSubscriptions()
        {
            var subscriptions = await _subscriptionService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionResource>>(subscriptions);
            return resources;
        }

        [SwaggerOperation(
            Summary = "List all Subscriptions of a Reader",
            Description = "List of Subscriptions of a Reader",
            OperationId = "ListAllSubscriptionsOfAReader"
        )]
        [SwaggerResponse(200, "List of Subscriptions for a Reader", typeof(SubscriptionResource))]
        [HttpGet("subscriber/{subscriberId}/subscribed")]
        public async Task<IEnumerable<UserResource>> GetAllBySubscriberIdAsync(long subscriberId)
        {
            var subscribed = await _userService.ListBySubscriberIdAsync(subscriberId);
            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(subscribed);
            return resources;
        }

        [SwaggerOperation(
            Summary = "Assign a subscription",
            Description = "Assign a subscription",
            OperationId = "AssignSubscription"
        )]
        [SwaggerResponse(200, "Subscription was Assigned", typeof(SubscriptionResource))]
        [HttpPost("subscriber/{subscriberId}/subscribed/{subscribedId}")]
        public async Task<IActionResult> AssignSubscriberSubscribed([FromBody] SaveSubscriptionResource subscriptionResource, long subscriberId, long subscribedId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            var subscription = _mapper.Map<SaveSubscriptionResource, Subscription>(subscriptionResource);
            var result = await _subscriptionService.AssignSubscriberSubscribedAsync(subscription,subscriberId, subscribedId);
            if (!result.Success)
                return BadRequest(result.Message);

            var subscribedResource = _mapper.Map<Subscription, SubscriptionResource>(result.Resource);
            return Ok(subscribedResource);

        }

        [SwaggerOperation(
            Summary = "Unassign Subscription",
            Description = "Unassign Subscription",
            OperationId = "UnassignSubscription"
        )]
        [SwaggerResponse(200, "Subscription was unassigned", typeof(SubscriptionResource))]
        [HttpDelete("subscriber/{subscriberId}/subscribed/{subscribedId}")]
        public async Task<IActionResult> UnassignSubscriberSubscribed(long subscriberId, long subscribedId)
        {
            var result = await _subscriptionService.UnassignSubscriberSubscribedAsync(subscriberId, subscribedId);

            if (!result.Success)
                return BadRequest(result.Message);
            var subscribedResource = _mapper.Map<Subscription, SubscriptionResource>(result.Resource);
            return Ok(subscribedResource);
        }
    }
}