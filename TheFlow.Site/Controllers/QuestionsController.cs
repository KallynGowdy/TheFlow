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
                string[] seperatedTags = question.Tags.Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                List<Tag> tags = new List<Tag>(seperatedTags.Length);
                foreach (string tag in seperatedTags)
                {
                    Tag t = dataContext.Tags.FirstOrDefault(a => a.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));
                    if (t == null)
                    {
                        t = new Tag(tag, null, ControllerHelper.Authenticate(Request, dataContext));
                    }
                    tags.Add(t);
                }

                Question q = new Question(ControllerHelper.Authenticate(Request, dataContext), question.Body, question.Title, tags);

                dataContext.Questions.Add(q);
                dataContext.SaveChanges();
                return RedirectToAction("Question", new { id = q.Id });
            }
            else
            {
                return View();
            }
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
            return ControllerHelper.Redirect(Url.Action("Index"), Request, Redirect);
        }
    }
}
