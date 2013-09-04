using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.Api.Entities;

namespace TheFlow.Site.Controllers
{
    /// <summary>
    /// Defines a controller that is used to perform basic operations on posts.
    /// </summary>
    public class PostsController : Controller
    {
        IDbContext dataContext = new DbContext();

        public PostsController() { }

        /// <summary>
        /// Creates a new PostsController using the given data context.
        /// </summary>
        /// <param name="dataContext"></param>
        public PostsController(IDbContext dataContext)
        {
            if (dataContext != null)
            {
                this.dataContext = dataContext;
            }
        }

        /// <summary>
        /// Adds an Up Vote to the post with the given id and redirects the user back to where they were.
        /// </summary>
        /// <param name="postId">The Id number of the post to Up Vote.</param>
        /// <returns>A Redirect Result that redirects the user to where they came from.</returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpVote([Bind(Prefix = "id")] long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                User user = ControllerHelper.Authenticate(Request, dataContext);

                //Make sure that the user is not voting on their own post
                if (user != null && post.Author.OpenId != user.OpenId)
                {
                    //Make sure that the user has not voted on the post yet
                    if (post.Votes.All(a => a.Voter.OpenId != user.OpenId))
                    {
                        post.Votes.Add(new TheFlow.Api.Entities.UpVote
                            {
                                Voter = user,
                                Post = post,
                                DateVoted = DateTime.UtcNow
                            });
                        dataContext.SaveChanges();
                    }
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        /// <summary>
        /// Removes the up/down vote that the current user did to the given post.
        /// </summary>
        /// <param name="postId">The Id number of the Post to Remove the vote from.</param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveVote([Bind(Prefix = "id")] long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                User user = ControllerHelper.Authenticate(Request, dataContext);
                if (user != null)
                {
                    //Find the vote from the user
                    Vote vote = post.Votes.SingleOrDefault(a => a.Voter.OpenId == user.OpenId);

                    if (vote != null)
                    {
                        post.Votes.Remove(vote);
                        dataContext.SaveChanges();
                    }
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        /// <summary>
        /// Adds a Down Vote to the post with the given Id based on the current user context.
        /// </summary>
        /// <param name="postId">The Id number of the Post to add a Down Vote to.</param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DownVote([Bind(Prefix = "id")] long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                User user = ControllerHelper.Authenticate(Request, dataContext);

                //make sure that the user is not voting on their own post
                if (user != null && post.Author.OpenId != user.OpenId)
                {
                    //Make sure that the user has not voted on the post yet
                    if (post.DownVotes.All(a => a.Voter.OpenId != user.OpenId))
                    {
                        post.Votes.Add(new DownVote
                        {
                            Voter = user,
                            DateVoted = DateTime.UtcNow,
                            Post = post
                        });
                        dataContext.SaveChanges();
                    }
                }
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
