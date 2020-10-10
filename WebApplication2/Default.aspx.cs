using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Limilabs.Mail;
using Limilabs.Mail.MSG;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebApplication2
{
    public partial class _Default : Page
    {
        string personalAccessToken = "4rm4d2q2wq452n7ce2mqgfj4yyqajoipmez63nvxoveijuphckxq";
        protected void Page_Load(object sender, EventArgs e)
        {
            // getrepo();
           // readEmail();
            DataTable workitems = getworkitems();
            DataTable pullrequests = getpullrequests();
            DataTable commits = getCommits();
            formTable(workitems, 1);
            formTable(pullrequests, 2);
            formTable(commits, 3);


            

        }

        private void formTable(DataTable workitems, int type)
        {
            StringBuilder sb = formTableStringBuilder(workitems);
            if (type == 1)
            {
                ltworkitems.Text = sb.ToString();
            }
            else if (type == 2)
            {
                ltpullrequests.Text = sb.ToString();
            }
            else
            {
                ltcommits.Text = sb.ToString();
            }
        }

        private StringBuilder formTableStringBuilder(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            //Table start.
            sb.Append("<table  class='table table-bordered'>");
            sb.Append("<thead  class='thead-dark'>");
            //Adding HeaderRow.
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th >" + column.ColumnName.ToString().ToUpper() + "</th>");
            }
            sb.Append("</tr>");
            sb.Append("</thead>");


            //Adding DataRow.
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td >" + row[column.ColumnName].ToString() + "</td>");
                }
                sb.Append("</tr>");
            }

            //Table end.
            sb.Append("</table>");
            return sb;
        }

        private async Task getrepo()
        {
            var personalaccesstoken = personalAccessToken;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalaccesstoken))));

                using (HttpResponseMessage response = client.GetAsync(
                            "https://dev.azure.com/codeletehackathon/secondproject/_apis/git/repositories?api-version=6.0").Result)
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
            }

        }

        private DataTable getpullrequests()
        {
            DataTable pullrequests;
            ListofPullRequests.pullRequests viewModel = null;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalAccessToken))));

                using (HttpResponseMessage response = client.GetAsync(
                            "https://dev.azure.com/codeletehackathon/secondproject/_apis/git/repositories/506f6d44-0a42-4a10-9f75-c89c450c3740/pullrequests?api-version=6.0").Result)
                {
                    response.EnsureSuccessStatusCode();
                    viewModel = response.Content.ReadAsAsync<ListofPullRequests.pullRequests>().Result;
                    List<PullReqItems> pullreqdetails = new List<PullReqItems>();
                    foreach (var vm in viewModel.value)
                    {
                        PullReqItems cm = new PullReqItems();
                        cm.reponame = vm.repository.name;
                        cm.creationDate = vm.creationDate;
                        cm.description = vm.description;
                        cm.createdBy = vm.createdby.displayName;
                        cm.title = vm.title;
                        pullreqdetails.Add(cm);
                    }
                    pullrequests = ToDataTable<PullReqItems>(pullreqdetails);
                }
            }
            return pullrequests;
        }

        public DataTable getCommits()
        {
            DataTable commits;
            ListofProjectsResponse.Projects viewModel = null;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalAccessToken))));

                using (HttpResponseMessage response = client.GetAsync(
                            "https://dev.azure.com/codeletehackathon/secondproject/_apis/git/repositories/506f6d44-0a42-4a10-9f75-c89c450c3740/commits?api-version=6.0").Result)
                {
                    response.EnsureSuccessStatusCode();
                    viewModel = response.Content.ReadAsAsync<ListofProjectsResponse.Projects>().Result;
                    List<commitdetails> commitdetails = new List<commitdetails>();
                    int i = 0;
                    foreach (var vm in viewModel.value)
                    {
                        if (i < 5)
                        {
                            commitdetails cm = new commitdetails();
                            cm.add = vm.changecounts.add;
                            cm.edit = vm.changecounts.edit;
                            cm.delete = vm.changecounts.delete;
                            cm.comment = vm.comment;
                            cm.author = vm.author.name;
                            cm.date = vm.author.date;
                            commitdetails.Add(cm);
                        }
                        i++;
                    }
                    commits = ToDataTable<commitdetails>(commitdetails);
                }
            }
            return commits;
        }

        public DataTable getworkitems()
        {
            DataTable dtworkitems;
            try
            {
                ListofWorkItems.WorkItems viewModel = null;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalAccessToken))));

                    using (HttpResponseMessage response = client.GetAsync(
                                "https://dev.azure.com/codeletehackathon/secondproject/_apis/wit/workitems?ids=4,6,7&api-version=6.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        viewModel = response.Content.ReadAsAsync<ListofWorkItems.WorkItems>().Result;
                        List<workitems> workdetails = new List<workitems>();
                        foreach (var vm in viewModel.value)
                        {
                            workitems wd = new workitems();
                            wd.AreaPath = vm.fields.AreaPath;
                            wd.WorkItemType = vm.fields.WorkItemType;
                            wd.CreatedDate = vm.fields.CreatedDate;
                            wd.Title = vm.fields.Title;
                            wd.Description = vm.fields.Description;
                            workdetails.Add(wd);
                        }
                        dtworkitems = ToDataTable<workitems>(workdetails);
                    }
                    return dtworkitems;
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }

        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public void readEmail()
        {

            using (MsgConverter converter = new MsgConverter(@"C:\Users\pramo_gsxqos1\Desktop\sample.msg"))
            {
                if (converter.Type == MsgType.Note)
                {
                    IMail email = converter.CreateMessage();

                    Console.WriteLine("Subject: {0}", email.Subject);
                    Console.WriteLine("Sender name: {0}", email.Sender.Name);
                    Console.WriteLine("Sender address: {0}", email.Sender.Address);

                    Console.WriteLine("Attachments: {0}", email.Attachments.Count);
                   
                }
            }
        
        }
    }
}