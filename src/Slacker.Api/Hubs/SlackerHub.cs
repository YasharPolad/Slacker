﻿using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Slacker.Application.Interfaces;
using Slacker.Application.Interfaces.RepositoryInterfaces;
using Slacker.Domain.Entities;
using System.Security.Claims;

namespace Slacker.Api.Hubs;

[Authorize]
public class SlackerHub : Hub
{
    private readonly IConnectionRepository _connectionRepository;
    private readonly ILogger<SlackerHub> _logger;

    public SlackerHub(IConnectionRepository connectionRepository, ILogger<SlackerHub> logger)
    {
        _connectionRepository = connectionRepository;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var connectionId = Context.ConnectionId;
        await addUserToGroupsAsync(connectionId, userId);
    }

    private async Task addUserToGroupsAsync(string connectionId, string userId)
    {
        List<Connection> connections = await _connectionRepository.GetAllAsync(c => 
                                                    c.Employees.Any(e => e.IdentityId == userId));
        foreach (var connection in connections)
        {
            string groupName = connection.Id.ToString(); //TODO: This looks a bit problematic
            await Groups.AddToGroupAsync(connectionId, groupName);
            _logger.LogInformation($"{userId} added to group {groupName}");
        }
    }

    public async Task SendMessageAsync(string message, string channelId, string ts, string replyTs)
    {
        var user = Context.User.FindFirst(ClaimTypes.Name) != null 
            ? Context.User.FindFirst(ClaimTypes.Name).Value 
            : "Anonymous User";

        
        await Clients.OthersInGroup(channelId).SendAsync("messageReceiver", message, user, ts, replyTs); //TODO: Only group members can send a message to a group!!!
    }


}
