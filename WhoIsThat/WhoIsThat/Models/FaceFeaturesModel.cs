using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class FaceFeaturesModel
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Age from face
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gender from face
        /// </summary>
        public string Gender { get; set; }
    }
}
