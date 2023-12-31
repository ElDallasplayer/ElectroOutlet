﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController
        public ActionResult Login(User user = null)
        {
            if (user.UserName == null || user.Password == null)
            {
                user = new User();
                user.UserName = "";
                user.Password = "";
                user.Id = -1;
                user.EsAdmin = false;
                user.Delete = false;

                return View("Login",user);
            }
            else
            {
                List<User> users = new User().GetUsers();

                if(users.Any(x => x.UserName.ToLower() == user.UserName.ToLower()))
                {
                    if (users.Any(x => x.UserName.ToLower() == user.UserName.ToLower() && x.Password == user.Password))
                    {
                        //LOGIN CORRECTO
                        User useronly = users.Where(x => x.UserName.ToLower() == user.UserName.ToLower()).First();
                        return RedirectToAction("Index", "Home", useronly);
                    }
                    else
                    {
                        //LOGIN INCORRECTO
                        ViewBag.DataResponse = "Password incorrecto";
                        return View(user);
                    }
                }
                else
                {
                    //NO EXISTE ESE USUARIO
                    ViewBag.DataResponse = "El usuario no existe";
                    return View(user);
                }
            }
        }

        [HttpPost]
        public ActionResult LogOut(int id)
        {
            List<User> users = new User().GetUsers();
            User us = users.Where(x => x.Id == id).FirstOrDefault();

            if (us != null)
            {
                return View("Login", us = null);
            }
            else
            {
                ViewBag.DataResponse = "El usuario no existe";
                return View("Login", us = null);
            }
        }
    }
}
