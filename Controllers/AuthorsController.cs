using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisher.Bookstore.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fisher.Bookstore.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
       private readonly BookstoreContext db;

       public AuthorsController(BookstoreContext db)
    {
        this.db = db;

        if (this.db.Authors.Count() == 0)
        {
            this.db.Authors.Add(new Author { 
                AuthorId = 1,
                Name = "Lemony Snicket"
            });

            this.db.Authors.Add(new Author {
                AuthorId = 2,
                Name = "Stephen King"
            });

            this.db.SaveChanges();
        }
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(db.Authors);
    }
    [HttpGet("{authorid}", Name="GetAuthor")]
    public IActionResult GetById(int authorid)
    {
        var author =db.Authors.Find(authorid);

        if(author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }
    [HttpPost]
    public IActionResult Post([FromBody]Author author)
    {
        if(author == null)
        {
            return BadRequest();
        }

        this.db.Authors.Add(author);
        this.db.SaveChanges();

        return CreatedAtRoute("GetAuthor", new { authorid = author.AuthorId}, author);
    }
    [HttpPut("{authorid}")]
    public IActionResult Put(int authorid, [FromBody]Author newAuthor)
    {
        if(newAuthor == null || newAuthor.AuthorId != authorid)
        {
            return BadRequest();
        }
        var currentAuthor = this.db.Authors.FirstOrDefault(x => x.AuthorId == authorid);

        if (currentAuthor == null)
        {
            return NotFound();
        }
        currentAuthor.Name = newAuthor.Name;

        this.db.Authors.Update(currentAuthor);
        this.db.SaveChanges();

        return NoContent();
    }
    [HttpDelete("{authorid}")]
    public IActionResult Delete(int authorid)
    {
        var author = this.db.Authors.FirstOrDefault(x => x.AuthorId == authorid);

        if (author == null)
        {
            return NotFound();
        }

        this.db.Authors.Remove(author);
        this.db.SaveChanges();

        return NoContent();
    }
    }
}      