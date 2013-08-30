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
        /// Deletes the answer with the given id from the database. Requires authentication from the user that created the answer.
        /// </summary>
        /// <param name="answerId">The Id number of the answer to delete.</param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete([Bind(Prefix="id")] long answerId)
        {
            User user = ControllerHelper.Authenticate(Request, dataContext);
            if (user != null)
            {
                Answer answer = dataContext.Answers.SingleOrDefault(a => a.Id == answerId);
                //make sure the answer exits and that the author was the current user
                if (answer != null && answer.Author.OpenId == user.OpenId)
                {
                    dataContext.Answers.Remove(answer);
                    dataContext.SaveChanges();
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        /// <summary>
        /// Creates or edits a new answer posted by the currently logged in user.
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
                Question question = dataContext.Questions.SingleOrDefault(a => a.Id == answer.QuestionId.Value);
                if (question != null && question.Answers.All(a => a.Author.OpenId != user.OpenId))
                {
                    Answer a = new Answer(user, answer.Body, question);

                    dataContext.Answers.Add(a);
                    dataContext.SaveChanges();
                    return RedirectToAction("Question", "Questions", new { id = answer.QuestionId });
                }
                else
                {
                    Answer a = dataContext.Answers.SingleOrDefault(ans => ans.Author.OpenId == user.OpenId);
                    if (a != null)
                    {
                        //Apply the edit
                        a.SetBody(answer.Body, user);
                        dataContext.SaveChanges();
                    }
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
