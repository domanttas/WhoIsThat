using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.Models
{
    public class TargetObject
    {
        int Id { get; set; }
        
        int HunterPersonId { get; set; }

        int PreyPersonId { get; set; }

        bool IsHunted { get; set; }
    }
}
