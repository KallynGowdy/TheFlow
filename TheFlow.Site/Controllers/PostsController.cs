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
using System.Data.Entity;
using TheFlow.Site.Models;
using TheFlow.Site.Authorization;

namespace TheFlow.Site.Controllers
{
    /// <summary>
    /// Defines a controller that is used to perform basic operations on posts.
    /// </summary>
    public class PostsController : Controller
    {
        IDbContext dataContext = new TheFlow.Api.Entities.DbContext();

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
        /// Creates a new edit for the post with the given id using the given post model.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "id")]long postId, PostModel newPost)
        {
            User user = ControllerHelper.GetAuthenticatedUser(dataContext);
            if (user != null)
            {
                Post post = dataContext.Posts.SingleOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    //Make edit because the editor is the author or because the editor has that permission
                    if (post.Author.OpenId == user.OpenId || UserPermissions.HasPermission(user, UserPermission.Edit))
                    {
                        post.SetBody(newPost.Body, user);
                    }
                    //propose edit
                    else
                    {
                        post.ProposeEdit(newPost.Body, user);
                    }
                }
                return ControllerHelper.RedirectBack(Request, Redirect, true);
            }
            else
            {
                return ControllerHelper.Redirect(Url.Action("LogIn", "Users"), Request, Redirect);
            }
        }


        /// <summary>
        /// Adds the given comment to the post with the given post id.
        /// </summary>
        /// <param name="postId">The Id number of the post to add the comment to.</param>
        /// <param name="comment">The Comment to add to the post.</param>
        /// <returns>A redirect back to the referring url</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Prefix = "id")]long postId, [System.Web.Http.FromBody]CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                User user = ControllerHelper.GetAuthenticatedUser(dataContext);
                if (user != null)
                {
                    Post post = dataContext.Posts.SingleOrDefault(p => p.Id == postId);
                    if (post != null)
                    {
                        post.Comments.Add(new Comment
                        {
                            Author = user,
                            Body = comment.Body,
                            DatePosted = DateTime.UtcNow,
                            Post = post
                        });
                        dataContext.SaveChanges();
                    }
                }
            }
            return ControllerHelper.RedirectBack(Request, Redirect, true);
        }

        /// <summary>
        /// Deletes the comment with the given id if it was posted by the current user.
        /// </summary>
        /// <param name="commentId">The Id number of the comment to delete.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public ActionResult DeleteComment([Bind(Prefix = "id")] long commentId)
        {
            User user = ControllerHelper.GetAuthenticatedUser(dataContext);

            if (user != null)
            {
                Comment comment = dataContext.Comments.SingleOrDefault(a => a.Id == commentId);
                if (comment != null && comment.Author.OpenId == user.OpenId)
                {
                    dataContext.Comments.Remove(comment);
                    dataContext.SaveChanges();
                }
            }
            return ControllerHelper.RedirectBack(Request, Redirect, true);
        }

        /// <summary>
        /// Serves the page with the post matching the given post id.
        /// </summary>
        /// <param name="postId">The Id number of the post to view.</param>
        [HttpGet]
        public ActionResult Index([Bind(Prefix = "id")]long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                if (post is Question)
                {
                    return ControllerHelper.Redirect(Url.Action("Question", "Questions", new { id = postId }), Request, Redirect);
                }
                else
                {
                    //Redirect 
                    return ControllerHelper.Redirect(string.Format("{0}#{1}", Url.Action("Question", "Questions", new { id = ((Answer)post).Question.Id }), postId.ToString()), Request, Redirect);
                }
            }
            else
            {
                return ControllerHelper.RedirectBack(Request, Redirect, true);
            }
        }

        /// <summary>
        /// Adds an Up Vote to the post with the given id and redirects the user back to where they were.
        /// </summary>
        /// <param name="postId">The Id number of the post to Up Vote.</param>
        /// <returns>A Redirect Result that redirects the user to where they came from.</returns>
        [ValidateAntiForgeryToken]
        public ActionResult UpVote([Bind(Prefix = "id")] long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                User user = ControllerHelper.GetAuthenticatedUser(dataContext);

                //Make sure that the user is not voting on their own post
                if (user != null && post.Author.OpenId != user.OpenId)
                {
                    //Make sure that the user has not voted on the post yet
                    if (post.Votes.All(a => a.Voter.OpenId != user.OpenId))
                    {
                        Vote vote = new TheFlow.Api.Entities.UpVote
                        {
                            Voter = user,
                            Post = post,
                            DateVoted = DateTime.UtcNow
                        };
                        int reputation = post.AddVote(vote);
                        vote.Value = reputation;
                        //add the reputation to the author
                        post.Author.Reputation += reputation;

                        dataContext.SaveChanges();
                    }
                    else
                    {
                        //otherwise, remove the vote.
                        return RemoveVote(postId);
                    }
                }
                else if (user == null)
                {
                    return ControllerHelper.Redirect(Url.Action("LogIn", "Users"), Request, Redirect);
                }
            }
            return Index(postId);
            //return ControllerHelper.RedirectBack(Request, Redirect, true);
        }

        /// <summary>
        /// Removes the up/down vote that the current user did to the given post.
        /// </summary>
        /// <param name="postId">The Id number of the Post to Remove the vote from.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public ActionResult RemoveVote([Bind(Prefix = "id")] long postId)
        {
            Post post = dataContext.Posts.SingleOrDefault(a => a.Id == postId);
            if (post != null)
            {
                User user = ControllerHelper.GetAuthenticatedUser(dataContext);
                if (user != null)
                {
                    //Find the vote from the user
                    Vote vote = post.Votes.SingleOrDefault(a => a.Voter.OpenId == user.OpenId);

                    if (vote != null)
                    {
                        int rep = post.RemoveVote(vote);
                        post.Author.Reputation += rep;
                        dataContext.SaveChanges();
                    }
                }
                else
                {
                    return ControllerHelper.Redirect(Url.Action("LogIn", "Users"), Request, Redirect);
                }
            }

            return Index(postId);
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
                User user = ControllerHelper.GetAuthenticatedUser(dataContext);

                //make sure that the user is not voting on their own post
                if (user != null && post.Author.OpenId != user.OpenId)
                {
                    //Make sure that the user has not voted on the post yet
                    if (post.DownVotes.All(a => a.Voter.OpenId != user.OpenId))
                    {
                        Vote vote = new TheFlow.Api.Entities.DownVote
                        {
                            Voter = user,
                            Post = post,
                            DateVoted = DateTime.UtcNow
                        };
                        int rep = post.AddVote(vote);
                        vote.Value = rep;
                        post.Author.Reputation += rep;
                        dataContext.SaveChanges();
                    }
                }
                else if (user == null)
                {
                    return ControllerHelper.Redirect(Url.Action("LogIn", "Users"), Request, Redirect);
                }
            }
            return Index(postId);
        }
    }
}
