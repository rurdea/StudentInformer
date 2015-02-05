using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInformerWebApp.Models
{
    public class ProjectHistory
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
        public ProjectStatus OldStatus { get; set; }
        public ProjectStatus NewStatus { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Comments { get; set; }
    }
}