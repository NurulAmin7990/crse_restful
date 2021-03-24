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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using RESTful_API.Models;

namespace RESTful_API.Controllers
{
    [RoutePrefix("api/families")]
    public class FamiliesController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public FamiliesController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        [Route("")]
        // GET: api/Families
        public List<FamilyViewModel> GetFamilies()
        {
            List<FamilyViewModel> families = new List<FamilyViewModel>();
            List<String> childrenUrl;
            List<String> parentUrl;

            foreach (Family family in db.Families.ToList())
            {
                childrenUrl = new List<string>();
                parentUrl = new List<string>();

                if (family.Children.Count() > 0)
                {
                    foreach (Child child in family.Children)
                    {
                        childrenUrl.Add(Url.Link("getChildren", new { id = child.ChildrenId }));
                    }
                }
                else
                {
                    childrenUrl.Add("No children");
                }

                if (family.Parents.Count() > 0)
                {
                    foreach (Parent parent in family.Parents)
                    {
                        parentUrl.Add(Url.Link("getParent", new { id = parent.ParentId }));
                    }
                }
                else
                {
                    parentUrl.Add("No parent");
                }

                families.Add(
                    new FamilyViewModel
                    {
                        ContactNumber = family.ContactNumber,
                        Email = family.Email,
                        AddressLine = family.AddressLine,
                        AddressArea = family.AddressArea,
                        AddressPostcode = family.AddressPostcode,
                        Children = childrenUrl,
                        Parent = parentUrl
                    }
                    );
            }
            return families;
        }

        [Route("{id}", Name ="getFamily")]
        // GET: api/Families/5

        [ResponseType(typeof(FamilyViewModel))]
        public IHttpActionResult GetFamily(int id)
        {
            Family family = db.Families.Find(id);
            List<String> childrenUrl = new List<string>();
            List<String> parentUrl = new List<string>();
            foreach (Child child in family.Children)
            {
                childrenUrl.Add(Url.Link("getChildren", new { id = child.ChildrenId }));
            }
            foreach (Parent parent in family.Parents)
            {
                parentUrl.Add(Url.Link("getParent", new { id = parent.ParentId }));
            }
            FamilyViewModel familyViewModel = new FamilyViewModel
            {
                ContactNumber = family.ContactNumber,
                Email = family.Email,
                AddressLine = family.AddressLine,
                AddressArea = family.AddressArea,
                AddressPostcode = family.AddressPostcode,
                Children = childrenUrl,
                Parent = parentUrl
            };
            if (family == null)
            {
                return NotFound();
            }

            return Ok(familyViewModel);
        }

        [Route("view")]
        // GET: api/Families
        [Authorize(Roles = "parent")]
        [ResponseType(typeof(FamilyViewModel))]
        public IHttpActionResult GetParentFamily()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Family family = db.Families.FirstOrDefault(f => f.Email == user.Email);
            List<String> childrenUrl = new List<string>();
            List<String> parentUrl = new List<string>();
            foreach (Child child in family.Children)
            {
                childrenUrl.Add(Url.Link("getChildren", new { id = child.ChildrenId }));
            }
            foreach (Parent parent in family.Parents)
            {
                parentUrl.Add(Url.Link("getParent", new { id = parent.ParentId }));
            }
            FamilyViewModel familyViewModel = new FamilyViewModel
            {
                ContactNumber = family.ContactNumber,
                Email = family.Email,
                AddressLine = family.AddressLine,
                AddressArea = family.AddressArea,
                AddressPostcode = family.AddressPostcode,
                Children = childrenUrl,
                Parent = parentUrl
            };
            if (family == null)
            {
                return NotFound();
            }

            return Ok(familyViewModel);
        }

        [Route("{id}")]
        // PUT: api/Families/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFamily(int id, Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != family.FamilyId)
            {
                return BadRequest();
            }

            db.Entry(family).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExists(id))
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
        // POST: api/Families
        [ResponseType(typeof(Family))]
        public IHttpActionResult PostFamily(Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Families.Add(family);
            db.SaveChanges();

            return CreatedAtRoute("getFamily", new { id = family.FamilyId }, family);
        }

        [Route("{id}")]
        // DELETE: api/Families/5
        [ResponseType(typeof(Family))]
        public IHttpActionResult DeleteFamily(int id)
        {
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return NotFound();
            }

            db.Families.Remove(family);
            db.SaveChanges();

            return Ok(family);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FamilyExists(int id)
        {
            return db.Families.Count(e => e.FamilyId == id) > 0;
        }
    }
}