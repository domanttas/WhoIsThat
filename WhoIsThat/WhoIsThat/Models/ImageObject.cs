using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class ImageObject : IComparable<ImageObject>
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public string ImageContentUri { get; set; }

        public string PersonFirstName { get; set; }

        public string PersonLastName { get; set; }

        public string DescriptiveSentence { get; set; }

        public int Score { get; set; }

        public int CompareTo(ImageObject another)
        {
            //return this.Score.CompareTo(another.Score);
            if (this.Score > another.Score) return -1;
            return this.Score < another.Score ? 1 : 0;
        }

    }
}
