namespace WebApplication2
{
    public class ListofProjectsResponse
    {
        public class Projects : BaseViewModel
        {
            public int count { get; set; }
            public Value[] value { get; set; }
        }

        public class Value
        {
            public string url { get; set; }
            public string comment { get; set; }
            public authrodetails author { get; set; }
            public changecount changecounts { get; set; }
        }      

    }
}