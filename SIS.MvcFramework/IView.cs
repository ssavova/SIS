﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IView
    {
        string GetHtml(object model, string userId);
    }
}
