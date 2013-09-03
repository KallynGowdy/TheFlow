﻿// Copyright 2013 Kallyn Gowdy
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
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using TheFlow.Api.Authentication;
using TheFlow.Api.Entities;
using TheFlow.Site.HtmlSanitization;

namespace TheFlow.Site.Controllers
{
    public static class ControllerHelper
    {
        #region HtmlSanitizer
        private static readonly HtmlSanitizer htmlSanitizer = new HtmlSanitizer
        {
            ElementFilter = new ElementFilter
            {
                DefaultMapType = ElementMapType.Disallow,
                FilterType = FilterType.FailOnFirst, //disallow the element if one of the attributes are wrong.
                MappedElements = new Dictionary<string, Tuple<ElementMapType, IAttributeFilter>>()
                {
                    {
                        #region A
		                "a", //allow hyperlinks
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow,
                                Attributes = new Dictionary<string, Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>>()
                                {
                                    {
                                        "href",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => 
                                                {
                                                    Uri result;
                                                    return Uri.TryCreate(a.Value, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttps || result.Scheme == Uri.UriSchemeHttp);
                                                })
                                        )
                                    },
                                    {
                                        "title",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => a.Value.All(c => char.IsLetterOrDigit(c)))
                                        )
                                    }
                                }
                            }
                        ) 
	#endregion
                    },
                    {
                        #region Img
		                "img", //allow image tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow,
                                Attributes = new Dictionary<string, Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>>()
                                {
                                    {
                                        "src",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => {
                                                Uri result;
                                                return Uri.TryCreate(a.Value, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
                                            })
                                        )
                                    },
                                    {
                                        "width",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => {
                                                int width;
                                                return int.TryParse(a.Value, out width) && (width > 0 && width <= 999);
                                            })
                                        )
                                    },
                                    {
                                        "height",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => {
                                                int height;
                                                return int.TryParse(a.Value, out height) && (height > 0 && height <= 999);
                                            })
                                        )
                                    },
                                    {
                                        "alt",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => {
                                                return a.Value.All(c => char.IsLetterOrDigit(c));
                                            })
                                        )
                                    },
                                    {
                                        "title",
                                        new Tuple<ElementMapType, Predicate<HtmlAgilityPack.HtmlAttribute>>
                                        (
                                            ElementMapType.Allow,
                                            (a => a.Value.All(c => char.IsLetterOrDigit(c)))
                                        )
                                    }
                                }
                            }
                        ) 
	#endregion
                    },
                    {
                        "b", //allow bold tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "blockquote", //allow blockquote tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "code", //allow code tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "em", //allow em tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "h1", //allow h1 tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "h2", //allow h2 tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "h3", //allow h3 tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "i", //allow italic tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "kbd", //allow kbd tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "li", //allow list item tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "ol", //allow ordered list tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "p", //allow paragraph tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "pre", //allow pre tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "s", //allow strikethrough tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "sup", //allow superscript tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "sub", //allow subscript tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "strong", //allow bold tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "strike", //allow strikethrough tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "ul", //allow unordered list tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "br", //allow line break tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    },
                    {
                        "hr", //allow horizontal rule tags
                        new Tuple<ElementMapType, IAttributeFilter>
                        (
                            ElementMapType.Allow,
                            new AttributeFilter
                            {
                                DefaultMapType = ElementMapType.Disallow
                            }
                        )
                    }
                }
            }
        }; 
        #endregion

        /// <summary>
        /// Gets the sanitizer to use for html.
        /// </summary>
        public static HtmlSanitizer HtmlSanitizer
        {
            get
            {
                return htmlSanitizer;
            }
        }

        /// <summary>
        /// Gets the OpenID Authentication provider by searching the form, headers, query string and then cookies.
        /// Returns null if the url could not be found.
        /// </summary>
        /// <returns></returns>
        public static string GetAuthProvider(HttpRequestBase request)
        {
            //check the form
            string url = request.Form["OpenIdProvider"];
            if (url == null)
            {
                //check the headers
                url = request.Headers["OpenIdProvider"];
                if (url == null)
                {
                    //check the query string
                    url = request.QueryString["OpenIdProvider"];
                    if (url == null)
                    {
                        //Check the cookies
                        HttpCookie cookie = request.Cookies["TheFlow-OpenIdProvider"];
                        if (cookie != null)
                        {
                            url = cookie.Value;
                        }
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// Determines if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity;
        }

        /// <summary>
        /// Authenticates the current user by either using forms authentication or repeated OpenID requests.
        /// If the user is not authenticated he/she will be redirected to their provider if the provider is supplied in the request.
        /// </summary>
        /// <returns>The currently authenticated user, or null if the user is not authenticated.</returns>
        public static User Authenticate(HttpRequestBase request, IDbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.SingleOrDefault(a => a.OpenId == ((FormsIdentity)HttpContext.Current.User.Identity).Name);
                return user;
            }
            else if (!FormsAuthentication.IsEnabled)
            {
                User user;
                if (AuthenticationServer.IsAuthenticated(out user))
                {
                    return user;
                }
                else
                {
                    string provider = GetAuthProvider(request);
                    if (provider != null)
                    {
                        AuthenticationServer.Authenticate(request, provider);
                    }
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the cookie with the given name from the users cookie collection.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static bool RemoveCookie(HttpRequestBase request, HttpResponseBase response, string cookieName)
        {
            if (request.Cookies[cookieName] != null)
            {
                HttpCookie newCookie = new HttpCookie(cookieName);
                newCookie.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Add(newCookie);
                return true;
            }
            return false;
        }
    }
}