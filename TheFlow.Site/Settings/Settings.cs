using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TheFlow.Site
{
    /// <summary>
    /// Provides a static and type-safe way to access settings from the Web.config.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Provides static settings for the Questions Controller.
        /// </summary>
        public static class QuestionController
        {
            private static readonly Dictionary<string, object> lookup = new Dictionary<string, object>()
            {
                {
                    "QuestionsPerPage",
                    int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.QuestionController.QuestionsPerPage"])
                }
            };

            /// <summary>
            /// Gets the number of questions to display per page at the questions index page.
            /// </summary>
            public static int QuestionsPerPage
            {
                get
                {
                    return (int)lookup["QuestionsPerPage"];
                }
            }
        }

    }
}