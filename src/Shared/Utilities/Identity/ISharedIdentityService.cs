using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Identity
{
    public interface ISharedIdentityService
    {
        string GetUserId { get; }
    }
}