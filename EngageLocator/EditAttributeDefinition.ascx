<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditAttributeDefinition.ascx.cs" Inherits="Engage.Dnn.Locator.EditAttributeDefinition" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<dnn:propertyeditorcontrol id="Attributes" runat="server" SortMode="SortOrderAttribute" ErrorStyle-cssclass="NormalRed" labelstyle-cssclass="SubHead" helpstyle-cssclass="Help" editcontrolstyle-cssclass="NormalTextBox" labelwidth="180px" editcontrolwidth="170px" width="350px" />
<p>
    <dnn:commandbutton class="CommandButton" id="cmdUpdate" onclick="cmdUpdate_Click" imageUrl="~/images/save.gif" resourcekey="cmdUpdate" runat="server" text="Update" />
    <dnn:commandbutton class="CommandButton" id="cmdCancel" onclick="cmdCancel_Click" imageUrl="~/images/lt.gif" resourcekey="cmdCancel" runat="server" text="Cancel" causesvalidation="False" />
    <dnn:commandbutton class="CommandButton" id="cmdDelete" onclick="cmdDelete_Click" imageUrl="~/images/delete.gif" resourcekey="cmdDelete" runat="server" text="Delete" causesvalidation="False" />
</p>