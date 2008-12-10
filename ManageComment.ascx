<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ManageComment.ascx.cs" Inherits="Engage.Dnn.Locator.ManageComment" %>

<table>
    <tr>
        <td>
            <asp:Label ID="lblCommentId" runat="server" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblLocation" runat="server" CssClass="SubHead" resourcekey="lblLocation" />
        </td>
        <td>
            <asp:Label ID="lblLocationTitle" runat="server" CssClass="normal" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblComment" runat="server" CssClass="SubHead" resourcekey="lblComment" />
        </td>
        <td>
            <asp:TextBox ID="txtComment" runat="server" CssClass="normal" TextMode="multiLine" Rows="5" MaxLength="200" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbltxtSubmittedBy" runat="server" CssClass="SubHead" resourcekey="lblSubmittedBy" />
        </td>
        <td>
            <asp:TextBox ID="txtSubmittedBy" runat="server" CssClass="normal" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblApproved" runat="server" CssClass="SubHead" resourcekey="lblApproved" />
        </td>
        <td>
            <asp:RadioButton ID="rbWaitingForApproval" runat="server" GroupName="rbCommentStatus" CssClass="normal" resourcekey="rbWaitingForApproval" />
            <asp:RadioButton ID="rbApproved" runat="server" GroupName="rbCommentStatus" CssClass="normal" resourcekey="rbApproved" />            
        </td>
    </tr>
    <tr>
        <td colspan="2" class="manageCommentButtons">
            <asp:Button ID="btnSaveComment" runat="server" CssClass="CommandButton" OnClick="btnSaveComment_Click" resourcekey="btnSaveComment" />
            <asp:Button ID="btnCancel" runat="server" CssClass="CommandButton" OnClick="btnCancel_Click" resourcekey="btnCancelComment" />
            <asp:Button ID="btnDelete" runat="server" CssClass="CommandButton"  OnClick="btnDelete_Click" resourcekey="btnDelete" />
        </td>
    </tr>
</table>