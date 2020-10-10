<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h3><b>Work Items</b></h3>
    <br />
    <asp:Literal ID="ltworkitems" runat="server" />

    <br />
    <h3><b>Pull Requests</b></h3>
    <br />
    <asp:Literal ID="ltpullrequests" runat="server" />
    <br />

    <h3><b>Commits</b></h3>
    <br />
    <asp:Literal ID="ltcommits" runat="server" />
    <br />




</asp:Content>
