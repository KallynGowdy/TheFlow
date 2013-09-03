using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFlow.Api.Entities;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for the preferences entity.
    /// </summary>
    public class PreferencesModel
    {
        /// <summary>
        /// Gets or sets the theme of code that the user should use.
        /// </summary>
        public CodeStyle? CodeTheme
        {
            get;
            set;
        }
    }
}
