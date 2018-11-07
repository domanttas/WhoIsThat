using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class TargetObject
    {
        public int Id { get; set; }
        
        public int HunterPersonId { get; set; }

        public int PreyPersonId { get; set; }

        public bool IsHunted { get; set; }
    }
}
