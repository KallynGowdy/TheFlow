// Copyright 2013 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.Api.Entities;
using TheFlow.Api.Models;

namespace TheFlow.Site.Controllers
{
    /// <summary>
    /// Defines a controller for creating/editing answers.
    /// </summary>
    public class AnswersController : Controller
    {
        IDbContext dataContext = new DbContext();

        public AnswersController()
        {
        }

        public AnswersController(IDbContext dataContext)
        {
            if (dataContext != null)
            {
                this.dataContext = dataContext;
            }
        }
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
        [ValidateInput(false)]
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
