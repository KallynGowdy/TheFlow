using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.API.Entities;
using TheFlow.API.Models;
using System.Data.Entity;

namespace TheFlow.Site.Controllers
{
    public class QuestionsController : Controller
    {
        TheFlow.API.Entities.DbContext dataContext = new TheFlow.API.Entities.DbContext();

        /// <summary>
        /// Serves the index of the top 50 questions to the user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(dataContext.Questions.Take(50));
        }

        /// <summary>
        /// Serves the view for the specified question.
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public ActionResult Question([Bind(Prefix = "id")] long questionId)
        {
            Question question = dataContext.Questions.Include(a => a.Author).FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                question.Views += 1;
                dataContext.SaveChanges();
                return View(new ViewQuestionModel(question));
            }
            return View("Index", dataContext.Questions.Include(a => a.Author).Take(50));
        }

        /// <summary>
        /// Serves the Create.cshtml page to the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                Question q = new Question
                {
                    Body = question.Body,
                    Title = question.Title,
                    DatePosted = DateTime.Now,
                    Author = ControllerHelper.Authenticate(Request, dataContext)
                };

                dataContext.Questions.Add(q);
                dataContext.SaveChanges();
            }
            else
            {
                return View();
            }
            return View("Index", dataContext.Questions.Take(50));
        }
    }
}
