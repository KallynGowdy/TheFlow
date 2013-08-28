using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Different code styles that the user can choose from.
    /// </summary>
    public enum CodeStyle
    {
        Default,
        Github,
        HemisuDark,
        HemisuLight,
        TomorrowNightBlue,
        TomorrowNightBright,
        TomorrowNightEighties,
        TomorrowNight,
        Tomorrow,
        VibrantInk,
        SolarizedDark,
        SonsOfObsidian,
        Desert,
        Doxy,
        Sunburst
    };

    /// <summary>
    /// Defines an entity for user preferences (Code Display Style, etc.)
    /// </summary>
    public class Preferences
    {
        /// <summary>
        /// Gets or sets the preferred code style to display to the user.
        /// </summary>
        public CodeStyle CodeStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Converts this entity to the related model.
        /// </summary>
        /// <returns></returns>
        public Models.PreferencesModel ToModel()
        {
            return new Models.PreferencesModel
            {
                CodeTheme = CodeStyle
            };
        }
    }
}
