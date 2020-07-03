using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Models
{
    public class CustomJobPost
    {
        public int PostID;
        public String Post;
        public CustomJobPost(JOB_POST jobPost)
        {
            this.PostID = jobPost.POST_ID;
            this.Post = jobPost.POST;
        }
    }
}