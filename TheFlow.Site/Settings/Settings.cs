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
        private static readonly Dictionary<string, object> lookup = new Dictionary<string, object>()
        {
            {
                "QuestionsPerPage",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.QuestionController.QuestionsPerPage"])
            },
            {
                "AnswerUpVoteReputationIncrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.AnswerUpVoteReputationIncrease"])
            },
            {
                "AnswerDownVoteReputationDecrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.AnswerDownVoteReputationDecrease"])
            },
            {
                "QuestionDownVoteReputationDecrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.QuestionDownVoteReputationDecrease"])
            },
            {
                "QuestionUpVoteReputationIncrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.QuestionUpVoteReputationIncrease"])
            },
            {
                "AnswerAcceptedReputationIncrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.AnswerAcceptedReputationIncrease"])
            },
            {
                "PostEditAcceptedReputationIncrease",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.PostEditAcceptedReputationIncrease"])
            },
            {
                "Permissions.UpVote",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.Permissions.UpVote"])
            },
            {
                "Permissions.DownVote",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.Permissions.DownVote"])
            },
            {
                "Permissions.Flag",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.Permissions.Flag"])
            },
            {
                "Permissions.Edit",
                int.Parse(ConfigurationManager.AppSettings["TheFlow.Site.Permissions.Edit"])
            },
        };

        /// <summary>
        /// Gets the name that should be asociated with the web site.
        /// </summary>
        public static string SiteName
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteName"];
            }
        }

        /// <summary>
        /// Gets the description that should be asociated with the web site.
        /// </summary>
        public static string SiteDescription
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteDescription"];
            }
        }

        /// <summary>
        /// Gets the Keywords that should be asociated with the web site.
        /// </summary>
        public static string SiteKeywords
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteKeywords"];
            }
        }

        /// <summary>
        /// Provides static settings for the Questions Controller.
        /// </summary>
        public static class QuestionController
        {
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

        /// <summary>
        /// Provides static settings for required reputation to access permissions.
        /// </summary>
        public static class Permissions
        {
            /// <summary>
            /// Gets the minimum ammount of reputation that is required to up vote posts.
            /// </summary>
            public static int UpVote
            {
                get
                {
                    return (int)lookup["Permissions.UpVote"];
                }
            }

            /// <summary>
            /// Gets the minimum ammount of reputation that is required to down vote posts.
            /// </summary>
            public static int DownVote
            {
                get
                {
                    return (int)lookup["Permissions.DownVote"];
                }
            }

            /// <summary>
            /// Gets the minimum ammount of reputation that is required to flag posts for problems.
            /// </summary>
            public static int Flag
            {
                get
                {
                    return (int)lookup["Permissions.Flag"];
                }
            }

            /// <summary>
            /// Gets the minimum ammount of reputation that is required to instantly edit posts.
            /// </summary>
            public static int Edit
            {
                get
                {
                    return (int)lookup["Permissions.Edit"];
                }
            }
        }

        /// <summary>
        /// Provides static settings for Reputation increases/decreases.
        /// </summary>
        public static class Reputation
        {
            /// <summary>
            /// Defines a class that defines reputation increase/decrease values for answers.
            /// </summary>
            public static class Answers
            {
                /// <summary>
                /// Gets the ammount of reputation to give the author of an answer that got upvoted.
                /// </summary>
                public static int UpVote
                {
                    get
                    {
                        return (int)lookup["AnswerUpVoteReputationIncrease"];
                    }
                }

                /// <summary>
                /// Gets the ammount of reputation to give the author of an answer that got downvoted.
                /// </summary>
                public static int DownVote
                {
                    get
                    {
                        return (int)lookup["AnswerDownVoteReputationDecrease"];
                    }
                }

                /// <summary>
                /// Gets the ammount of reputation to award the author of an answer that got accepted.
                /// </summary>
                public static int Accepted
                {
                    get
                    {
                        return (int)lookup["AnswerAcceptedReputationIncrease"];
                    }
                }
            }

            /// <summary>
            /// Defines a class that defines reputation increase/decrease values for questions.
            /// </summary>
            public static class Questions
            {
                /// <summary>
                /// Gets the ammount of reputation to give the author of a question that got downvoted.
                /// </summary>
                public static int DownVote
                {
                    get
                    {
                        return (int)lookup["QuestionDownVoteReputationDecrease"];
                    }
                }

                /// <summary>
                /// Gets the ammount of reputation to give the author of a question that got upvoted.
                /// </summary>
                public static int UpVote
                {
                    get
                    {
                        return (int)lookup["QuestionUpVoteReputationIncrease"];
                    }
                }
            }

            /// <summary>
            /// Defines a class that defines reputation increase/decrease values for posts.
            /// </summary>
            public static class Posts
            {
                /// <summary>
                /// Gets the ammount of reputation to award a user whose edit on a post got accepted.
                /// </summary>
                public static int EditAccepted
                {
                    get
                    {
                        return (int)lookup["PostEditAcceptedReputationIncrease"];
                    }
                }
            }
        }
    }
}