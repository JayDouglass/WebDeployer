<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebDeployer.Web.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gridPendingDeployments" runat="server"></asp:GridView>
    <asp:Button ID="btnTest" runat="server" Text="Button" onclick="btnTest_Click"></asp:Button>
</asp:Content>
