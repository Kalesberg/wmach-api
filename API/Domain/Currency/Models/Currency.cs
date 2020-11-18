using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// Currency
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        /// <value>
        /// The currency identifier.
        /// </value>
        public Int64 currencyID { get; set; }

        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        /// <value>
        /// The currency symbol.
        /// </value>
        public string currencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the currency abbreviation.
        /// </summary>
        /// <value>
        /// The currency abbreviation.
        /// </value>
        public string currencyAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the currency description.
        /// </summary>
        /// <value>
        /// The currency description.
        /// </value>
        public string currencyDescription { get; set; }

        /// <summary>
        /// Gets or sets the currency format string.
        /// </summary>
        /// <value>
        /// The currency format string.
        /// </value>
        public string currencyFormatString { get; set; }
    }
}