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
            else
            {
                ParticipantViewModel participantViewModel = new ParticipantViewModel
                {
                    @event = Url.Link("getEvent", new { id = participant.EventId }),
                    children = Url.Link("getChildren", new { id = participant.ChildrenId }),
                    Lane = participant.Lane,
                    Time = participant.Time
                };
                return Ok(participantViewModel);
            }
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
        [ResponseType(typeof(Participant))]
        public IHttpActionResult PostParticipant(Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Participants.Add(participant);
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