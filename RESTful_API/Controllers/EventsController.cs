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
    [RoutePrefix("api/events")]
    public class EventsController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: api/Events
        [Route("")]
        public List<EventViewModel> GetEvents()
        {
            List<EventViewModel> events = new List<EventViewModel>();

            foreach (Event @event in db.Events.ToList())
            {
                events.Add(
                    new EventViewModel
                    {
                        Meet = Url.Link("getMeet", new {id = @event.MeetId }),
                        AgeRange = @event.AgeRange,
                        Gender = @event.Gender,
                        Distance = @event.Distance,
                        Stroke = @event.Stroke,
                        Round = @event.Round,
                        StartTime = @event.StartTime,
                        EndTime = @event.EndTime
                    });
            }
            return events;
        }

        // GET: api/Events/5
        [Route("{id}", Name = "getEvent")]
        [ResponseType(typeof(EventViewModel))]
        public IHttpActionResult GetEvent(int id)
        {

            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            else
            {
                EventViewModel eventViewModel = new EventViewModel
                {
                    Meet = Url.Link("getMeet", new { id = @event.MeetId}),
                    AgeRange = @event.AgeRange,
                    Gender = @event.Gender,
                    Distance = @event.Distance,
                    Stroke = @event.Stroke,
                    Round = @event.Round,
                    StartTime = @event.StartTime,
                    EndTime = @event.EndTime
                };
                return Ok(eventViewModel);
            }
        }

        // PUT: api/Events/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEvent(int id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        [Route("")]
        [ResponseType(typeof(Event))]
        public IHttpActionResult PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return CreatedAtRoute("getEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [Route("{id}")]
        [ResponseType(typeof(Event))]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;
        }
    }
}