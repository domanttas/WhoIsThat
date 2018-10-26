using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class ImageObject
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public string ImageContentUri { get; set; }

        public string PersonFirstName { get; set; }

        public string PersonLastName { get; set; }

        public string PersonDescription { get; set; }
    }
}
