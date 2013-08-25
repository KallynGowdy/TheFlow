using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.API.Entities;
using TheFlow.API.Models;

namespace TheFlow.Site.Controllers
{
    public class QuestionsController : Controller
    {
        DbContext dataContext = new DbContext();

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
            Question question = dataContext.Questions.FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                return View(question);
            }
            return View("Index", dataContext.Questions.Take(50));
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
            Question q = new Question
            {
                Body = question.Body,
                Title = question.Title,
                DatePosted = DateTime.Now,
                Author = ControllerHelper.authenticate(Request, dataContext)
            };

            dataContext.Questions.Add(q);
            dataContext.SaveChanges();

            return View("Index", dataContext.Questions.Take(50));
        }
    }
}
