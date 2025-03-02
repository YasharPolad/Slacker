﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Slacker.Application.Interfaces;
using Slacker.Application.Interfaces.RepositoryInterfaces;
using System.Security.Claims;

namespace Slacker.Api.Authorization;

public class PostModifyRequirement : IAuthorizationRequirement
{

}

public class PostModifyRequirementHandler : AuthorizationHandler<PostModifyRequirement>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPostRepository _postRepository;

    public PostModifyRequirementHandler(IHttpContextAccessor contextAccessor,
           IPostRepository postRepository)
    {
        _contextAccessor = contextAccessor;
        _postRepository = postRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PostModifyRequirement requirement)
    {
        if(!context.User.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
            await Task.CompletedTask;

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var postId = int.Parse(_contextAccessor.HttpContext.GetRouteValue("id").ToString());
        var post = await _postRepository.GetAsync(p => p.Id == postId, p => p.Employee);
        if (post.Employee.IdentityId == userId)
            context.Succeed(requirement);
        
        await Task.CompletedTask;
    }
}
