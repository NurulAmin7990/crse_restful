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
    [RoutePrefix("api/children")]
    public class ChildrenController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        [Route("")]
        // GET: api/Children
        public List<SwimmerViewModel> GetChildren()
        {
            List<SwimmerViewModel> childs = new List<SwimmerViewModel>();

            foreach (Child child in db.Children.ToList())
            {
                childs.Add(
                    new SwimmerViewModel
                    {
                        Firstname = child.Firstname,
                        Lastname = child.Lastname,
                        DateOfBirth = child.DateOfBirth,
                        Gender = child.Gender,
                        Permission = child.Permission,
                        FamilyUrl = Url.Link("getFamily", new { id = child.Family.FamilyId })
                    }
                    );
            }
            return childs;
        }

        [Route("{id}", Name = "getChildren")]
        // GET: api/Children/5
        [ResponseType(typeof(SwimmerViewModel))]
        public IHttpActionResult GetChild(int id)
        {
            Child child = db.Children.Find(id);
            SwimmerViewModel swimmerViewModel = new SwimmerViewModel
            {
                Firstname = child.Firstname,
                Lastname = child.Lastname,
                DateOfBirth = child.DateOfBirth,
                Gender = child.Gender,
                Permission = child.Permission,
                FamilyUrl = Url.Link("getFamily", new { id = child.Family.FamilyId })
            };
            if (child == null)
            {
                return NotFound();
            }

            return Ok(swimmerViewModel);
        }

        [Route("{id}")]
        // PUT: api/Children/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChild(int id, Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.ChildrenId)
            {
                return BadRequest();
            }

            db.Entry(child).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("")]
        // POST: api/Children
        [ResponseType(typeof(Child))]
        public IHttpActionResult PostChild(Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Children.Add(child);
            db.SaveChanges();

            return CreatedAtRoute("getChildren", new { id = child.ChildrenId }, child);
        }

        [Route("{id}")]
        // DELETE: api/Children/5
        [ResponseType(typeof(Child))]
        public IHttpActionResult DeleteChild(int id)
        {
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return NotFound();
            }

            db.Children.Remove(child);
            db.SaveChanges();

            return Ok(child);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChildExists(int id)
        {
            return db.Children.Count(e => e.ChildrenId == id) > 0;
        }
    }
}