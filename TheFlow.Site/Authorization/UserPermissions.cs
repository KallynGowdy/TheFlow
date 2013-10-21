using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFlow.Api.Entities;

namespace TheFlow.Site.Authorization
{
    /// <summary>
    /// Defines a list of permissions that a User can have.
    /// </summary>
    public enum UserPermission
    {
        /// <summary>
        /// Defines that a user with this permission can Up Vote posts.
        /// </summary>
        UpVote,
        /// <summary>
        /// Defines that a user with this permission can Down Vote posts.
        /// </summary>
        DownVote,
        /// <summary>
        /// Defines that a user can flag a post for problems.
        /// </summary>
        Flag,
        /// <summary>
        /// Defines a permission that a user can edit a post with instant acceptance.
        /// </summary>
        Edit,
        /// <summary>
        /// Defines a permission that a user can comment on any post.
        /// </summary>
        Comment
    }

    /// <summary>
    /// Defines a static helper class that provides functions related to user permissions.
    /// </summary>
    public static class UserPermissions
    {
        /// <summary>
        /// Determines if the given user has (can use) the provided permission.
        /// </summary>
        /// <param name="user">The user to test against.</param>
        /// <param name="permission">The permission to test for access to.</param>
        /// <returns></returns>
        public static bool HasPermission(User user, UserPermission permission)
        {
            switch (permission)
            {
                case UserPermission.UpVote:
                    return user.Reputation >= Settings.Permissions.UpVote;
                case UserPermission.DownVote:
                    return user.Reputation >= Settings.Permissions.DownVote;
                case UserPermission.Flag:
                    return user.Reputation >= Settings.Permissions.Flag;
                case UserPermission.Edit:
                    return user.Reputation >= Settings.Permissions.Edit;
                case UserPermission.Comment:
                    return user.Reputation >= Settings.Permissions.Comment;
            }
            return false;
        }
    }
}