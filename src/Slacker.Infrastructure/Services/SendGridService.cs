﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Slacker.Application.Interfaces;
using Slacker.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slacker.Infrastructure.Services;
public class SendGridService : IEmailService
{
    private readonly ISendGridClient _sendgridClient;
    private readonly IConfiguration _configuration;

    public SendGridService(ISendGridClient sendgridClient, IConfiguration configuration, ILogger<SendGridService> logger)
    {
        _sendgridClient = sendgridClient;
        _configuration = configuration;
    }

    public async Task<EmailResponse> SendMailAsync(string recipient, string heading, string body)
    {
        var from = new EmailAddress(_configuration["SendgridSettings:Sender"]); 
        var to = new EmailAddress(recipient);
        var plainTextContent = body;
        var htmlContent = body;
        var subject = heading;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await _sendgridClient.SendEmailAsync(msg);  //TODO: Maybe have an exception thrown here in case email isn't sent

        return new EmailResponse { StatusCode = response.StatusCode.ToString(), Body = response.Body.ToString() };        
        
    }
}
