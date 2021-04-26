using System;
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
    [RoutePrefix("api/parents")]
    public class ParentsController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();
        [Route("")]
        // GET: api/Parents
        public List<ParentViewModel> GetParents()
        {
            List<ParentViewModel> parents = new List<ParentViewModel>();

            foreach (Parent parent in db.Parents.ToList())
            {
                parents.Add(
                    new ParentViewModel { 
                        Firstname = parent.Firstname,
                        Lastname = parent.Lastname,
                        DateOfBirth = parent.DateOfBirth,
                        Gender = parent.Gender,
                        FamilyUrl = Url.Link("getFamily", new { id = parent.Family.FamilyId})
                        }
                    );
            }
            return parents;
        }

        [Route("{id}", Name = "getParent")]
        // GET: api/Parents/5
        [ResponseType(typeof(ParentViewModel))]
        public IHttpActionResult GetParent(int id)
        {
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return NotFound();
            }
            ParentViewModel parentViewModel = new ParentViewModel
            {
                Firstname = parent.Firstname,
                Lastname = parent.Lastname,
                DateOfBirth = parent.DateOfBirth,
                Gender = parent.Gender,
                FamilyUrl = Url.Link("getFamily", new { id = parent.Family.FamilyId })
            };

            return Ok(parentViewModel);
        }

        [Route("{id}")]
        // PUT: api/Parents/5
        [Authorize(Roles = "staff")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParent(int id, Parent parent)
        {
            if (id != parent.ParentId)
            {
                return BadRequest();
            }
                db.Entry(parent).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
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
        // PUT: api/Parent
        [Authorize(Roles = "parent")]

        [ResponseType(typeof(Parent))]
        public IHttpActionResult PutParent(string number)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Family family = db.Families.FirstOrDefault(f => f.Email == user.Email);
            if (family == null)
            {
                return NotFound();
            }
            family.ContactNumber = number;
            db.Entry(family).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtRoute("getFamily", new { id = family.FamilyId }, family);
        }

        [Route("")]
        // POST: api/Parents
        [Authorize(Roles = "staff")]
        [ResponseType(typeof(Parent))]
        public IHttpActionResult PostParent(Parent parent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Parents.Add(parent);
            db.SaveChanges();

            return CreatedAtRoute("getParent", new { id = parent.ParentId }, parent);
        }

        [Route("{id}")]
        // DELETE: api/Parents/5
        [ResponseType(typeof(Parent))]
        public IHttpActionResult DeleteParent(int id)
        {
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return NotFound();
            }

            db.Parents.Remove(parent);
            db.SaveChanges();

            return Ok(parent);
        }

        [Route("Archive/{id}")]
        [ResponseType(typeof(ArchiveParentViewModel))]
        [Authorize(Roles = "staff")]
        //POST: api/Parents/Archive/2
        public IHttpActionResult PostArchive(int id)
        {
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return NotFound();
            }
            Archive archive = new Archive
            {
                DateOfBirth = parent.DateOfBirth,
                Firstname = parent.Firstname,
                Lastname = parent.Lastname,
                Gender = parent.Gender,
                Type = "Parent"
            };
            db.Archives.Add(archive);
            db.Parents.Remove(parent);
            db.SaveChanges();
            ArchiveParentViewModel archiveView = new ArchiveParentViewModel
            {
                Firstname = archive.Firstname,
                Lastname = archive.Lastname,
                DateOfBirth = archive.DateOfBirth,
                Gender = archive.Gender
            };
            return Ok(archiveView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParentExists(int id)
        {
            return db.Parents.Count(e => e.ParentId == id) > 0;
        }
    }
}