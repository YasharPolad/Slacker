﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slacker.Application.Models.DTOs;
public class FileDto
{
    public string FilePath { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}
