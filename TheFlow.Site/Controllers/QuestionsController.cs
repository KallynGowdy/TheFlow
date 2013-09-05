using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.Api.Entities;
using TheFlow.Api.Models;
using System.Data.Entity;

namespace TheFlow.Site.Controllers
{
    public class QuestionsController : Controller
    {
        TheFlow.Api.Entities.IDbContext dataContext = new TheFlow.Api.Entities.DbContext();

        public QuestionsController()
        {
        }

        public QuestionsController(TheFlow.Api.Entities.IDbContext dataContext)
        {
            if (dataContext != null)
            {
                this.dataContext = dataContext;
            }
        }

        /// <summary>
        /// Serves the index of the top 50 questions to the user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(dataContext.Questions.Take(50).Include(a => a.Edits).ToArray());
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
            return View("Index", dataContext.Questions.Include(a => a.Author).Take(50).Include(a => a.Edits));
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                Question q = new Question(ControllerHelper.Authenticate(Request, dataContext), question.Body, question.Title);

                dataContext.Questions.Add(q);
                dataContext.SaveChanges();
            }
            else
            {
                return View();
            }
            return View("Index", dataContext.Questions.Take(50).Include(a => a.Edits));
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public ActionResult Delete([Bind(Prefix = "id")] long questionId)
        {
            Question question = dataContext.Questions.SingleOrDefault(a => a.Id == questionId);
            if (question != null)
            {
                User user = ControllerHelper.Authenticate(Request, dataContext);
                if (user != null && user.OpenId == question.Author.OpenId)
                {
                    dataContext.Questions.Remove(question);
                    dataContext.SaveChanges();
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
