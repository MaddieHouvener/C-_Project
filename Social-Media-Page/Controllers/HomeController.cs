using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // [HttpGet("dashboard")]
        // public IActionResult Dashboard()
        // {
        //     return View();
        // }

        //out of convienience. helper method --------------------------------------------
        public User GetCurrentUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return null;
            }
            return _context.Users
                .First(u => u.UserId == userId);
        }

        //--------------------------------------------------------------------
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Users.Add(newUser);

                _context.SaveChanges();
                //create a session to stay logged in
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("SocialMedia"); // or success?
            }
            else
            {   //have to use return not redirect to render validations
                ViewBag.ErrorMessage = "Please correct the required fields";
                return View("Index");
            }
        }

        //--------------------------------------------------------------------

        [HttpPost("login")]
        public IActionResult LoginPage(Login userSubmission)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);

            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Please check your email and password");
                    return View("Index");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please Correct the required fields";
                return View("Index");
            }

            //if the passwords match, store the UserId in session
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("SocialMedia");
        }


        // [HttpGet("dashboard")]
        // public IActionResult Dashboard()
        // {
        //     // pass user info to the view

        //     var CurrentUser = GetCurrentUser();
        //     //so they cant get to a page if not logged in
        //     if (CurrentUser == null)
        //     {
        //         return RedirectToAction("Index");
        //     }

        //     ViewBag.CurrentUser = CurrentUser;

        //     ViewBag.AllMessages = _context.Messages
        //         .Include(u => u.User)
        //         .Include(u => u.Comments)
        //         .OrderBy(w => w.CreatedAt).LastOrDefault();

        //     ViewBag.AllComments = _context.Comments
        //         .Include(c => c.Users)
        //         .Include(c => c.Messages)
        //         .OrderBy(c => c.CreatedAt).LastOrDefault();

        //     return View();
        // }

        [HttpPost("postMessage")]
        public IActionResult PostMessageAction(Message newMessage)
        {
            newMessage.UserId = (int)HttpContext.Session.GetInt32("UserId");

            _context.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("SocialMedia");
        }

        [HttpPost("postComment")]
        public IActionResult PostComment(Comment newComment)
        {
            newComment.UserId = (int)HttpContext.Session.GetInt32("UserId");


            Console.WriteLine(newComment.MessageId);
            Console.WriteLine(newComment.UserId);
            Console.WriteLine(newComment.Comments);
            Console.WriteLine(newComment.Name);
            Console.WriteLine(newComment.CommentId);


            _context.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("SocialMedia");
        }

        [HttpPost("dashboard/{messageId}/delete")]
        public IActionResult DeleteMessage(int messageId)
        {
            var messageToDelete = _context.Messages
                .First(m => m.MessageId == messageId);

            _context.Remove(messageToDelete);
            _context.SaveChanges();
            return RedirectToAction("SocialMedia");
        }

        [HttpGet("socialmedia")]
        public IActionResult SocialMedia()
        {

            var CurrentUser = GetCurrentUser();
            //so they cant get to a page if not logged in
            if (CurrentUser == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentUser = CurrentUser;

            ViewBag.AllMessages = _context.Messages
                .Include(u => u.User)
                .Include(u => u.Comments)
                    .ThenInclude(c => c.Users)
                .OrderByDescending(w => w.CreatedAt);

            ViewBag.AllComments = _context.Comments
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .OrderBy(c => c.CreatedAt);
            return View();
        }

        [HttpGet("friendRequests")]
        public IActionResult FriendRequest()
        {
            return View();
        }

        [HttpPost("media/{messageId}/like")]
        public IActionResult Like(int messageId)
        {
            var CurrentUser = GetCurrentUser();

            var like = new Message
            {
                UserId = CurrentUser.UserId,
                MessageId = messageId
            };

            _context.Add(like);
            _context.SaveChanges();

            return RedirectToAction("SocialMedia");
        }


        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
