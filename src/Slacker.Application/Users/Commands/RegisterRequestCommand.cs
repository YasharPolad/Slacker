﻿using MediatR;
using Slacker.Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Slacker.Application.Users.Commands;
public class RegisterRequestCommand : IRequest<BaseMediatrResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
}
