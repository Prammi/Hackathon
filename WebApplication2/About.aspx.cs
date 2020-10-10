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
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class About : Page
    {
        DataTable dtTextAnalytic = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            dtTextAnalytic.Columns.Add("Parameter");
            dtTextAnalytic.Columns.Add("Value");

        }
        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (FileUpLoad1.HasFile)
            {

                FileUpLoad1.SaveAs(@"C:\temp\" + FileUpLoad1.FileName);
               Label1.Text = "File Uploaded: " + FileUpLoad1.FileName;
                readEmail();
                formTable();
            }
            else
            {
                Label1.Text = "No File Uploaded.";
            }
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
                    string Mailbody = email.GetBodyAsText();

                    getKeyPhrases(Mailbody);

                }
            }

        }


        private void formTable()
        {
            StringBuilder sb = formTableStringBuilder(dtTextAnalytic);
            ltTextAnalytics.Text = sb.ToString();
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

        private void getKeyPhrases(string mailbody)
        {
            const string collectionUri = "https://dev.azure.com/codeletehackathon";
            const string projectName = "secondproject";
            const string repoName = "secondproject";
            const string pat = "4rm4d2q2wq452n7ce2mqgfj4yyqajoipmez63nvxoveijuphckxq";

            var creds = new VssBasicCredential(string.Empty, pat);

            // Connect to Azure DevOps Services
            var connection = new VssConnection(new Uri(collectionUri), creds);

            // Get a GitHttpClient to talk to the Git endpoints
            var gitClient = connection.GetClient<GitHttpClient>();

            // Get data about a specific repository
            var repo = gitClient.GetRepositoryAsync(projectName, repoName).Result;
            var repoid = repo.Id;
            string collectionuris = "https://dev.azure.com/codeletehackathon/secondproject/_apis/git/repositories/506f6d44-0a42-4a10-9f75-c89c450c3740/commits?api-version=6.0";

            //String text = "Thank you for helping your project manager, Angela Parmar, on the latest project. Your work reflects your dedication to the project and the organisation.";

            AmazonComprehendClient comprehendClient = new AmazonComprehendClient(Amazon.RegionEndpoint.USWest2);

            // Call DetectKeyPhrases API
            Console.WriteLine("Calling DetectKeyPhrases");
            DetectKeyPhrasesRequest detectKeyPhrasesRequest = new DetectKeyPhrasesRequest()
            {
                Text = mailbody,
                LanguageCode = "en"
            };
            DetectKeyPhrasesResponse detectKeyPhrasesResponse = comprehendClient.DetectKeyPhrases(detectKeyPhrasesRequest);
            DetectSyntaxRequest dsr;
            DetectSyntaxResponse dsresp;
            List<string> lkeyPhrases =new List<string>();
            string strKeyPhrases = string.Empty;
            foreach (KeyPhrase kp in detectKeyPhrasesResponse.KeyPhrases)
            {
                lkeyPhrases.Add(kp.Text);
                strKeyPhrases = strKeyPhrases  + ',' + kp.Text;
            }
            var values = new object[2];
            values[0] = "Key Phrases";
            values[1] = strKeyPhrases.Substring(1);
            dtTextAnalytic.Rows.Add(values);
            Console.WriteLine("Done");
        }
    }
}