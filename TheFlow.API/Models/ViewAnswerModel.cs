﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFlow.API.Entities;
using TheFlow.API;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for an answer to a question that is going to be viewed (not created or edited) by a user.
    /// </summary>
    public class ViewAnswerModel : ViewPostModel
    {
        /// <summary>
        /// Gets or sets the question that this answer is for.
        /// </summary>
        public ViewQuestionModel Question
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wether this answer is accepted.
        /// </summary>
        /// <returns></returns>
        public bool IsAccepted
        {
            get;
            set;
        }

        public ViewAnswerModel()
        {

        }

        public ViewAnswerModel(Answer answer, ViewQuestionModel question = null)
            : base(answer)
        {
            if (question == null)
            {
                this.Question = new ViewQuestionModel(answer.Question);
            }
            else
            {
                this.Question = question;
            }
        }
    }
}