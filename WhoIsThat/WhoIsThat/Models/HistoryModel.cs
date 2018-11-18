using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class HistoryModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TargetId { get; set; }

        public string Status { get; set; }
    }
}
