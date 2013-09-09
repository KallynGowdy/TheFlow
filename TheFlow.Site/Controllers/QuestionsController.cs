using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.Api.Entities;
using TheFlow.Api.Models;
using System.Data.Entity;
using System.ComponentModel;

namespace TheFlow.Site.Controllers
{
    public class QuestionsController : Controller
    {
        TheFlow.Api.Entities.IDbContext dataContext = new TheFlow.Api.Entities.DbContext();

        /// <summary>
        /// Creates a new Questions Controller that uses a new TheFlow.Api.Entities.DbContext instance for data access.
        /// </summary>
        public QuestionsController()
        {
        }

        /// <summary>
        /// Creates a new Questions Controller using the given data context for data access.
        /// </summary>
        /// <param name="dataContext">The IDbContext object to use for data access.</param>
        public QuestionsController(TheFlow.Api.Entities.IDbContext dataContext)
        {
            if (dataContext != null)
            {
                this.dataContext = dataContext;
            }
        }

        /// <summary>
        /// Gets the questions that reside on the given page based on the number of pages to view per page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private IEnumerable<Question> getQuestions(int page)
        {
            int skipNum = page * Settings.QuestionController.QuestionsPerPage;
            return dataContext.Questions.OrderByDescending(a => a.DatePosted).Skip(skipNum).Take(Settings.QuestionController.QuestionsPerPage).Include(a => a.Edits).ToArray();
        }

        /// <summary>
        /// Serves the index of the top 50 questions to the user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index([DefaultValue(0)]int page = 0)
        {
            //Return the current page, total pages, and questions to show
            return View(new Tuple<int, int, IEnumerable<Question>>(page, dataContext.Questions.Count() / Settings.QuestionController.QuestionsPerPage, getQuestions(page)));
        }

        /// <summary>
        /// Serves the view for the specified question.
        /// </summary>
        /// <param name="questionId">The Id number of the question to view.</param>
        /// <param name="addView">Whether to add a view to the question.</param>
        /// <returns></returns>
        public ActionResult Question([Bind(Prefix = "id")] long questionId, bool addView = false)
        {
            Question question = dataContext.Questions.Include(a => a.Author).FirstOrDefault(q => q.Id == questionId);
            if (question != null)
            {
                if (addView)
                {
                    question.Views += 1;
                    dataContext.SaveChanges();
                }
                return View(new ViewQuestionModel(question));
            }
            return ControllerHelper.RedirectBack(Request, Redirect);
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
            return ControllerHelper.RedirectBack(Request, Redirect);
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
            return RedirectToAction("Index");
        }
    }
}
