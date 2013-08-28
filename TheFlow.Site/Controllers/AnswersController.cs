using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.API.Entities;
using TheFlow.API.Models;

namespace TheFlow.Site.Controllers
{
    /// <summary>
    /// Defines a controller for creating/editing answers.
    /// </summary>
    public class AnswersController : Controller
    {
        DbContext dataContext = new DbContext();

        /// <summary>
        /// Creates a new answer posted by the currently logged in user.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(AnswerModel answer)
        {
            User user = ControllerHelper.Authenticate(Request, dataContext);
            if (user != null && answer != null && ModelState.IsValid)
            {
                Question question = dataContext.Questions.FirstOrDefault(a => a.Id == answer.QuestionId.Value);
                if (question != null && question.Answers.All(a => a.Author.OpenId != user.OpenId))
                {
                    Answer a = new Answer
                    {
                        Author = user,
                        Body = answer.Body,
                        DatePosted = DateTime.Now,
                        Question = question
                    };

                    dataContext.Answers.Add(a);
                    dataContext.SaveChanges();
                    return RedirectToAction("Question", "Questions", new { id = answer.QuestionId });
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
