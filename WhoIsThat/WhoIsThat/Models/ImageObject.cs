using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class ImageObject
    {
        int Id { get; set; }

        string ImageName { get; set; }

        string ImageContentUri { get; set; }

        string PersonFirstName { get; set; }

        string PersonLastName { get; set; }
    }
}
