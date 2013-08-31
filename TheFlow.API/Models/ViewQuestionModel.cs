using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for questions that are going to be viewed (not created or edited) by a user.
    /// </summary>
    public class ViewQuestionModel : ViewPostModel
    {

        public ViewQuestionModel(Entities.Question question) : base(question)
        {
            this.Title = question.Title;
            this.AcceptedAnswer = question.AcceptedAnswer != null ? new ViewAnswerModel(question.AcceptedAnswer, this) : null;
            this.Answers = question.Answers.Select(a => new ViewAnswerModel(a, this));
        }

        public ViewQuestionModel() { }

        /// <summary>
        /// Gets or sets the tags of the question.
        /// </summary>
        public IEnumerable<ViewTagModel> Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the question.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the accepted answer to the question.
        /// </summary>
        public ViewAnswerModel AcceptedAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of answers to this question.
        /// </summary>
        public IEnumerable<ViewAnswerModel> Answers
        {
            get;
            set;
        }
    }
}