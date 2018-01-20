using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;

namespace PlexLander.Data
{
    public class PlexSessionRepository : IPlexSessionRepository
    {
        private PlexLanderContext _context;

        public PlexSessionRepository(PlexLanderContext context)
        {
            _context = context;
        }

        public void Update(string email, string token, string username, DateTime sessionStart, string thumbnail = "", IEnumerable<PlexServer> servers = null)
        {
            var session = _context.PlexSessions.Find(email, token);

            session.Username = username;
            session.SessionStart = sessionStart;
            if ((session.Servers != null && session.Servers.Count > 0) &&
                servers != null)
            {
                // we've been passed servers and there are servers present
                if (servers.Count() > 0)
                {
                    //we've been told to clear all saved servers
                    session.Servers.Clear();
                }
                else
                {
                    //add the servers to the current ones
                    session.Servers.AddRange(servers);
                }
            }

            _context.PlexSessions.Update(session);
        }

        public void Save(string email, string token, string username, DateTime sessionStart, string thumbnail = "", IEnumerable<PlexServer> servers = null)
        {
            var session = new PlexAuthentication
            {
                Email = email,
                Token = token,
                Username = username,
                SessionStart = sessionStart
            };
            if (servers != null && servers.Count() > 0)
            {
                //we've gotten a list of servers to save right of the bat
                session.Servers = new List<PlexServer>(servers);
            }


            if (!string.IsNullOrWhiteSpace(thumbnail)) // check if we need to update the thumbnail
            {
                session.Thumbnail = thumbnail;
            }

            _context.PlexSessions.Add(session);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets a query containing all sessions for a certain email address.
        /// </summary>
        /// <param name="email">the email address</param>
        /// <returns>An IQueryable of PlexAuthentication objects.</returns>
        public IQueryable<PlexAuthentication> GetSessionsForEmail(string email)
        {
            return _context.PlexSessions.Where(s => s.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Deletes any session older than DateTime.Now - sessionAge
        /// </summary>
        /// <param name="sessionAge">The maximum age of the session</param>
        public void DeleteOldSessions(TimeSpan sessionAge)
        {
            var timeOut = DateTime.Now - sessionAge;
            var sessionQuery = _context.PlexSessions.Where(s => s.SessionStart.CompareTo(timeOut) < 0);
            if (sessionQuery.Any())
            {
                _context.PlexSessions.RemoveRange(sessionQuery);
                _context.SaveChanges();
            }
        }
    }
}
