using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentInformerWebApp.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set;}
        public string Year { get; set; }
        public string Semester { get; set; }
        public string Subject { get; set; }
        public string PhisicalPath { get; set; }
        public string Url { get; set; }
        public string UploadedBy { get; set; }
        public DateTime DateUploaded { get; set; }
        public int DownloadCount { get; set; }
    }
}