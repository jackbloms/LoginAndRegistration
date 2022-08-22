using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginAndRegis.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginAndRegis.Controllers;

public class UserController : Controller
{
    //db is just a variable name
    private UserContext db;

    //use this and bool loggedIn, to validate that our user is signed in,
    //before giving them access to those routes
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }

    //here we are "injecting" our context service into the constructor
    public UserController(UserContext context)
    {
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View("Register");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if(ModelState.IsValid)
        {
            if(db.Users.Any(user => user.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "is taken");
            }
        }

        if(ModelState.IsValid == false)
        {
            return Index();
        }

        //now we hash our password
        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        //saving userID in session
        //now that we've savedChanges, we have access to UserId from SQL
        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return View("Login");
    }

    [HttpGet("/view/login")]
    public IActionResult ViewLogin()
    {
        return View("Login");
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if(ModelState.IsValid == false)
        {
            return Index();
        }

        User? dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if(dbUser == null)
        {
            //normally login validations should be more generic to
            //avoid phishing, but we're using specific error
            //messages for testing
            ModelState.AddModelError("LoginEmail", "Not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwResult = hashBrowns.VerifyHashedPassword
        (loginUser, dbUser.Password, loginUser.LoginPassword);

        if(pwResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "is not corrent");
            return Index();
        }

        //no returns, therefore no errors
        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return View("Success");
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Register");
    }
}