using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RESTful_API.Models;

namespace RESTful_API.Controllers
{
    [RoutePrefix("api/meets")]
    public class MeetsController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        [Route("")]
        // GET: api/Meets
        public List<MeetsViewModel> GetMeets()
        {
            List<MeetsViewModel> meets = new List<MeetsViewModel>();
            List<String> eventUrl;
            foreach (Meet meet in db.Meets.ToList())
            {
                eventUrl = new List<string>();
                
                if (meet.Events.Count() > 0)
                {
                    foreach (Event ev in meet.Events)
                    {
                        eventUrl.Add(Url.Link("getEvent", new { id = ev.EventId }));
                    }
                }
                else
                {
                    eventUrl.Add("No event");
                }

                meets.Add(
                    new MeetsViewModel
                    {
                        Name = meet.Name,
                        Venue = meet.Venue,
                        Date = meet.Date,
                        PoolLength = meet.PoolLength,
                        Events = eventUrl
                    }
                    );
            }
            return meets;
        }

        [Route("{id}", Name = "getMeet")]
        // GET: api/Meets/5
        [ResponseType(typeof(MeetsViewModel))]
        public IHttpActionResult GetMeet(int id)
        {
            Meet meet = db.Meets.Find(id);
            List<String> eventUrl;

            if (meet == null)
            {
                return NotFound();
            }
            else
            {
                eventUrl = new List<string>();
                if (meet.Events.Count() > 0)
                {
                    foreach (Event ev in meet.Events)
                {

                    eventUrl.Add(Url.Link("getEvent", new { id = ev.EventId }));
                }
                }
                else
                {
                    eventUrl.Add("No event");
                }

                MeetsViewModel meetsViewModel = new MeetsViewModel
                {
                    Name = meet.Name,
                    Venue = meet.Venue,
                    Date = meet.Date,
                    PoolLength = meet.PoolLength,
                    Events = eventUrl
                };
                return Ok(meetsViewModel);
            }
        }
        [Route("{id}")]
        // PUT: api/Meets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeet(int id, Meet meet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meet.MeetId)
            {
                return BadRequest();
            }

            db.Entry(meet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [Route("")]
        // POST: api/Meets
        [Authorize(Roles = "staff")]
        [ResponseType(typeof(Meet))]
        public IHttpActionResult PostMeet(Meet meet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Meets.Add(meet);
            db.SaveChanges();

            return CreatedAtRoute("getMeet", new { id = meet.MeetId }, meet);
        }

        [Route("{id}")]
        // DELETE: api/Meets/5
        [ResponseType(typeof(Meet))]
        public IHttpActionResult DeleteMeet(int id)
        {
            Meet meet = db.Meets.Find(id);
            if (meet == null)
            {
                return NotFound();
            }

            db.Meets.Remove(meet);
            db.SaveChanges();

            return Ok(meet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeetExists(int id)
        {
            return db.Meets.Count(e => e.MeetId == id) > 0;
        }
    }
}