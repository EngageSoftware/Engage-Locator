<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ManageLocation.ascx.cs" Inherits="Engage.Dnn.Locator.ManageLocation" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<div runat="server" id="singleDivError" visible="false">
    <asp:Label runat="server" ID="singleError">Please go to Module settings page and add a Location Type prior to adding a Location.</asp:Label>
    <asp:Label ID="lblError" runat="server" CssClass="Normal"></asp:Label>
</div>
<div class="importPanelSingle" id="divPanelSingle" runat="server" >
    <div class="importPanelSingleCenter">
                <table id="tblSettings" class="importTableSingle" cellspacing="0" cellpadding="2"
                    border="0" align="center">
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblLocationId" runat="server" text="Key:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationId" CssClass="Normal" runat="server" TabIndex="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLocationId" runat="server" ControlToValidate="txtLocationId"
                                ErrorMessage="Please Enter Key" ValidationGroup="addLocation">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblName" runat="server" text="Location Name:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" CssClass="Normal" runat="server" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Please Enter Name of Location"
                                ControlToValidate="txtName" ValidationGroup="addLocation">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblWebsite" runat="server" text="Website:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtWebsite" CssClass="Normal" runat="server" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblAddress1" runat="server" text="Address:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress1" CssClass="Normal" runat="server" TabIndex="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ErrorMessage="Please Enter Address"
                                ControlToValidate="txtAddress1" ValidationGroup="addLocation">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblAddress2" runat="server" text="Address (cont):" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress2" CssClass="Normal" runat="server" TabIndex="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblCity" runat="server" text="City:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" CssClass="Normal" runat="server" TabIndex="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ErrorMessage="Please select a City"
                                ControlToValidate="txtCity" ValidationGroup="addLocation">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblState" runat="server" text="State:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlState" CssClass="Normal" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
                                AutoPostBack="True" TabIndex="7">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="cvState" runat="server" ControlToValidate="ddlState" ErrorMessage="Please select a State"
                                Operator="NotEqual" ValueToCompare="-1" ValidationGroup="addLocation">*</asp:CompareValidator></td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px; height: 47px;">
                            <dnn:label id="lblZip" runat="server" text="Postal Code:" />
                            &nbsp;
                        </td>
                        <td style="height: 47px">
                            <asp:TextBox ID="txtZip" CssClass="Normal" runat="Server" TabIndex="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvZip" runat="server" ErrorMessage="Please select a PostalCode"
                                ControlToValidate="txtZip" ValidationGroup="addLocation">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>        
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblCountry" runat="server" text="Country:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCountry" CssClass="Normal" runat="server" TabIndex="9">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="cvCountry" runat="server" ControlToValidate="ddlCountry"
                                ErrorMessage="Please Select a Country" Operator="NotEqual" ValueToCompare="-1"
                                ValidationGroup="addLocation">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px; height: 47px;">
                            <dnn:label id="lblLatitude" runat="server" text="Latitude:" />
                            &nbsp;
                        </td>
                        <td style="height: 47px">
                            <asp:TextBox ID="txtLatitude" CssClass="Normal" runat="Server" TabIndex="10"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px; height: 47px;">
                            <dnn:label id="lblLongitude" runat="server" text="Longitude:" />
                            &nbsp;
                        </td>
                        <td style="height: 47px">
                            <asp:TextBox ID="txtLongitude" CssClass="Normal" runat="Server" TabIndex="11"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px; height: 42px;">
                            <dnn:label id="lblPhone" runat="server" text="Phone Number:" />
                        </td>
                        <td style="height: 42px">
                            <asp:TextBox ID="txtPhone" CssClass="Normal" runat="server" TabIndex="12"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblLocationDetails" runat="server" text="Location Details:" />
                        </td>
                        <td>
                            <dnn:TextEditor id="teLocationDetails" runat="server" HtmlEncode="false" TextRenderMode="Raw" Width="500px" Height="400px" ChooseMode="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:label id="lblType" runat="server" text="Location Type" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlType" CssClass="Normal" runat="server" TabIndex="13" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlType"
                                ErrorMessage="Please Select a Location Type" Operator="NotEqual" ValueToCompare="-1"
                                ValidationGroup="addLocation">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" style="width: 150px">
                            <dnn:Label ID="lblStatus" runat="server" Text="Status" Visible="false" />
                        </td>
                        <td class="rbStatus">
                            <asp:RadioButton ID="rbWaitingForApproval" runat="server" Text="Waiting For Approval" resourcekey="rbWaitingForApproval" GroupName="rblStatus" CssClass="Normal" />
                            <asp:RadioButton ID="rbApprove" runat="server" Text="Approve" resourcekey="rbApprove" GroupName="rblStatus" CssClass="Normal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead">
                            <dnn:Label ID="lblAttributes" runat="server" Visible="false" />
                        </td>
                        <td>
                            <table>
                                <asp:Repeater ID="rptCustomAttributes" runat="server" Visible="false" OnItemDataBound="rptCustomAttributes_ItemDataBound" >
                                    <HeaderTemplate>
                                        <tr>
			                                <th class="thcustomAttributeHead"><asp:Label id="lblCustomAttributeHeader" runat="server" Text="Attribute Name" resourcekey="lblCustomAttributeHeader" /></th>
			                                <th class="thcustomAttributeHead"><asp:Label id="lblCustomAttributeValue" runat="server" Text="Attribute Value" resourcekey="lblCustomAttributeValue" /></th>
			                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnLocationAttributeID" runat="server" Visible="false" />
                                                <asp:HiddenField ID="hdnAttributeDefinitionId" runat="server" Visible="false" />
                                                <asp:Label ID="lblCustomAttribute" runat="server" CssClass="Normal" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomAttribute" runat="server" CssClass="Normal" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                            </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                </table>
        <div id="Buttons">
                <asp:TextBox ID="txtId" runat="server" Visible="False" CssClass="SubHead"></asp:TextBox>
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="addLocation"
                    resourcekey="btnSubmit" CssClass="CommandButton" TabIndex="14" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" resourcekey="btnCancelComment"
                    CssClass="CommandButton" TabIndex="15" />&nbsp;
                <asp:Button ID="btnDelete" runat="server" Text="Delete" resourcekey="btnDelete" CssClass="CommandButton"
                    TabIndex="16" OnClick="btnDelete_Click" Visible="false" />
        </div>
    </div>
</div>
<asp:ValidationSummary ID="vldSummary" runat="server" ValidationGroup="addLocation" />
