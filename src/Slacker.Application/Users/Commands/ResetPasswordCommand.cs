﻿using MediatR;
using Slacker.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slacker.Application.Users.Commands;
public class ResetPasswordCommand : IRequest<BaseMediatrResult>
{
    public string NewPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
