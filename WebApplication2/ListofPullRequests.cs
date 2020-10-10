using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    public class ListofPullRequests
    {
        public class pullRequests : BaseViewModel
        {
            
            public Value[] value { get; set; }
        }

        public class Value
        {
            public Repositorys repository { get; set; }
            public string creationDate { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public createdbys createdby { get; set; }

        }
    }
}