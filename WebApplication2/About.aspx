<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication2.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Text Analytics</h3>
    <br />
    
     <asp:FileUpload id="FileUpLoad1" runat="server" class="btn-success" /> 
    <br />
    <asp:label id ="Label1" runat="server"></asp:label>
    <br/>
    <asp:Button id="UploadBtn" Text="Upload File" OnClick="UploadBtn_Click" runat="server" Width="105px"  Height="40px  " class="btn btn-success"/>
      <br /><br />
    <asp:Literal ID="ltTextAnalytics" runat="server" />




<style>
/*    div {
        position: relative;
        overflow: hidden;
    }

    input {
        position: absolute;
        font-size: 50px;
        opacity: 0;
        right: 0;
        top: 0;
    }*/
</style>
</asp:Content>
