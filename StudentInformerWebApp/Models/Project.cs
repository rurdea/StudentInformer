using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInformerWebApp.Models
{
    public enum ProjectStatus
    {
        Created,
        Incomplete,
        Complete
    }

    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Reviewer { get; set; }
        public ProjectStatus Status { get; set; }
        public string Grade { get; set; }
        public DateTime DateUploaded { get; set; }
        public string PhisicalPath { get; set; }
        public string Url { get; set; }
    }
}