using System;
using System.Collections.Generic;
using System.Linq;
using PlexLander.Models;

namespace PlexLander.Data
{
    public interface IPlexSessionRepository
    {
        IQueryable<PlexAuthentication> GetSessionsForEmail(string email);
        void Save(string email, string token, string username, DateTime sessionStart, string thumbnail = "", IEnumerable<PlexServer> servers = null);
        void DeleteOldSessions(TimeSpan sessionAge);
    }
}