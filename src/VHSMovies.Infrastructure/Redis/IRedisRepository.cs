﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace VHSMovies.Infrastructure.Redis
{
    public interface IRedisRepository
    {
        Task SetAsync(string key, string value);
        Task<string?> GetAsync(string key);
    }
}
