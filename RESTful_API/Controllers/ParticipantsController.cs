using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RESTful_API.Models;

namespace RESTful_API.Controllers
{
    [RoutePrefix("api/participants")]
    public class ParticipantsController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: api/Participants
        [Route("")]
        public List<ParticipantViewModel> GetParticipants()
        {
            List<ParticipantViewModel> participants = new List<ParticipantViewModel>();

            foreach (Participant participant in db.Participants.ToList())
            {
                participants.Add(new ParticipantViewModel
                {
                    @event = Url.Link("getEvent", new { id = participant.EventId }),
                    children = Url.Link("getChildren", new { id = participant.ChildrenId }),
                    Lane = participant.Lane,
                    Time = participant.Time
                });
            }
            return participants;
        }

        // GET: api/Participants/5
        [Route("{id}", Name = "getParticipant")]
        [ResponseType(typeof(ParticipantViewModel))]
        public IHttpActionResult GetParticipant(int id)
        {
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return NotFound();
            }
            ParticipantViewModel participantViewModel = new ParticipantViewModel
            {
                @event = Url.Link("getEvent", new { id = participant.EventId }),
                children = Url.Link("getChildren", new { id = participant.ChildrenId }),
                Lane = participant.Lane,
                Time = participant.Time
            };
            return Ok(participantViewModel);
        }

        // GET: api/Participants/5
        [Route("personal")]
        [Authorize(Roles = "swimmer")]
        [ResponseType(typeof(SwimmerCustomViewModel))]
        public IHttpActionResult GetSwimmerView()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Participant participant = db.Participants.FirstOrDefault(p => p.Child.Family.Email == user.Email);
            if (participant == null)
            {
                return Content(HttpStatusCode.NotFound, "You are not participating in any events.");
            }
            List<EventViewModel> events = new List<EventViewModel>();
            List<MeetsViewModel> meets = new List<MeetsViewModel>();
            List<ParticipantViewModel> races = new List<ParticipantViewModel>();

            races.Add(new ParticipantViewModel
            {
                @event = Url.Link("getEvent", new { id = participant.EventId }),
                Lane = participant.Lane,
                Time = participant.Time
            });

            foreach (Event e in db.Events)
            {
                if(e.EventId == participant.EventId)
                {
                    events.Add(new EventViewModel
                    {
                        AgeRange = e.AgeRange,
                        Distance = e.Distance,
                        EndTime = e.EndTime,
                        Gender = e.Gender,
                        Round = e.Round,
                        Meet = e.Meet.Name,
                        StartTime = e.StartTime,
                        Stroke = e.Stroke
                    });
                    foreach (Meet m in db.Meets.ToList())
                    {
                        if (m.MeetId == e.MeetId)
                        {
                            meets.Add(new MeetsViewModel
                            {
                                Name = m.Name,
                                Date = m.Date,
                                PoolLength = m.PoolLength,
                                Venue = m.Venue
                            });
                        }
                    }
                }
            }
            SwimmerCustomViewModel swimmerCustomViewModel = new SwimmerCustomViewModel
            {
                Name = participant.Child.Firstname + " " + participant.Child.Lastname,
                DateOfBirth = participant.Child.DateOfBirth,
                Gender = participant.Child.Gender,
                Races = races,
                Events = events,
                Meets = meets
            };
            return Ok(swimmerCustomViewModel);
        }

        // PUT: api/Participants/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParticipant(int id, Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.ParticipantId)
            {
                return BadRequest();
            }

            db.Entry(participant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        // POST: api/Participants
        [Route("")]
        [Authorize(Roles = "staff")]
        [ResponseType(typeof(Participant))]
        public IHttpActionResult PostParticipant(Participant participant)
        {
            Child child = db.Children.Find(participant.ChildrenId);
            Event @event = db.Events.Find(participant.EventId);
            int age = DateTime.Today.Year - child.DateOfBirth.Year;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(child == null && @event == null)
            {
                return NotFound();
            }
            else
            {
                if (child.Permission == true && age <= @event.AgeRange)
                {
                    db.Participants.Add(participant);
                    db.SaveChanges();
                    return CreatedAtRoute("getParticipant", new { id = participant.ParticipantId }, participant);
                }
                else
                {
                    return Ok(new { response = "Swimmer does not meet criteria, try again." });
                }
            }
        }

        // PUT: api/Participants/1
        [Route("{id}")]
        [Authorize(Roles = "staff")]
        [ResponseType(typeof(Participant))]
        public IHttpActionResult PutParticipantTime(int id, TimeSpan time)
        {
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return NotFound();
            }
            participant.Time = time;
            db.Entry(participant).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtRoute("getParticipant", new { id = participant.ParticipantId }, participant);
        }

        // DELETE: api/Participants/5
        [Route("{id}")]
        [ResponseType(typeof(Participant))]
        public IHttpActionResult DeleteParticipant(int id)
        {
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return NotFound();
            }

            db.Participants.Remove(participant);
            db.SaveChanges();

            return Ok(participant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParticipantExists(int id)
        {
            return db.Participants.Count(e => e.ParticipantId == id) > 0;
        }
    }
}