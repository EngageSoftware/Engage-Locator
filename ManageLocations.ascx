<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.ManageLocations" AutoEventWireup="True" CodeBehind="ManageLocations.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="div_ManagementButtons">
    <asp:ImageButton ID="lbSettings" CssClass="CommandButton" runat="server" AlternateText="Settings" ImageUrl="~/desktopmodules/EngageLocator/images/settingsBt.gif" OnClick="lbSettings_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageLocations" CssClass="CommandButton" runat="server" AlternateText="Manage Locations" ImageUrl="~/desktopmodules/EngageLocator/images/locationbt.gif" OnClick="lbManageLocations_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbImportFile" CssClass="CommandButton" runat="server" AlternateText="Import File" ImageUrl="~/desktopmodules/EngageLocator/images/importbt.gif" OnClick="lbImportFile_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbManageComments" CssClass="CommandButton" runat="server" AlternateText="Comments" ImageUrl="~/desktopmodules/EngageLocator/images/commentsbt.gif" OnClick="lbManageComments_OnClick" CausesValidation="false" />
    <asp:ImageButton ID="lbLocationTypes" CssClass="CommandButton" runat="server" AlternateText="Location Types" OnClick="lbManageTypes_OnClick" ImageUrl="~/desktopmodules/EngageLocator/images/locationTypesBt.gif" CausesValidation="false" />
</div>

<asp:Label ID="lblConfigured" runat="server" CssClass="Normal" Text="Module is not Configured. Please go to Module Settings and configure module before managing locations." Visible="False" resourcekey="lblConfigured"></asp:Label>

<div class="divPanelTab" id="divPanelTab" runat="server">
    <asp:ImageButton runat="server" ID="btnAddLocation" CssClass="CommandButton" AlternateText="Add New Location" OnClick="btnAddLocation_Click" ImageUrl="~/desktopmodules/EngageLocator/images/caCreateNew.gif" />
    <br />
    <asp:Label ID="lblSuccess" runat="server" CssClass="Normal"></asp:Label>
    <br />
    <asp:Label runat="server" CssClass="Normal" ID="lblEditLocations" resourcekey="lblEditLocations" Visible="false">Edit Locations</asp:Label>
    <asp:UpdatePanel ID="upDataImport" runat="server">
        <ContentTemplate>
            <div id="editDiv" runat="server">
                <div id="divApproval" runat="server" class="div_approval">
                    <asp:RadioButton ID="rbApproved" runat="server" GroupName="approval"
                        resourcekey="rbApproved" CssClass="Normal" OnCheckedChanged="rbApproved_CheckChanged"
                        AutoPostBack="true" />
                    <asp:RadioButton ID="rbWaitingForApproval" runat="server" GroupName="approval"
                        resourcekey="rbWaitingForApproval" CssClass="Normal" OnCheckedChanged="rbApproved_CheckChanged"
                        AutoPostBack="true" />
                </div>
                <dnn:PagingControl ID="pager" runat="server"></dnn:PagingControl>
                <asp:DataGrid ID="dgLocations" runat="server" CssClass="importData" 
                    GridLines="Vertical" OnDataBinding="dgLocations_DataBind" 
                    AutoGenerateColumns="False" OnEditCommand="dgLocations_EditCommand" OnCancelCommand="dgLocations_CancelCommand"
                    OnItemDataBound="dgLocations_ItemDataBound"
                    OnItemCreated="dgLocations_ItemCreated" AllowSorting="True" 
                    DataKeyField="LocationId" onsortcommand="dgLocations_SortCommand">
                    <FooterStyle BackColor="#ccc" ForeColor="Black" />
                    <PagerStyle CssClass="dataImportPage" HorizontalAlign="Center" Mode="NumericPages" />
                    <HeaderStyle CssClass="dataImportHeader" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblLocationId" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.LocationId", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                       <%-- <asp:TemplateColumn HeaderText="Key">
                            <ItemTemplate>
                                <asp:Label ID="lblLoc" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.ExternalIdentifier", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="idDataGridFooter" />
                            <FooterStyle CssClass="idDataGridFooter" />
                            <HeaderStyle CssClass="idDataGridFooter" />
                        </asp:TemplateColumn>--%>
                        <asp:TemplateColumn HeaderText="Name" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Name", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="nameDataGridFooter" />
                            <FooterStyle CssClass="nameDataGridFooter" />
                            <HeaderStyle CssClass="nameDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Address" SortExpression="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Address", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="addressDataGridFooter" />
                            <FooterStyle CssClass="addressDataGridFooter" />
                            <HeaderStyle CssClass="addressDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="City" SortExpression="City">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.City", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="cityDataGridFooter" />
                            <FooterStyle CssClass="cityDataGridFooter" />
                            <HeaderStyle CssClass="cityDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Region" SortExpression="StateName">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.StateName", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="stateDataGridFooter" />
                            <FooterStyle CssClass="stateDataGridFooter" />
                            <HeaderStyle CssClass="stateDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Latitude" SortExpression="Latitude">
                            <ItemTemplate>
                                <asp:Label ID="lblLatitude" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Latitude") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="latitudeDataGridFooter" />
                            <FooterStyle CssClass="latitudeDataGridFooter" />
                            <HeaderStyle CssClass="latitudeDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Longitude" SortExpression="Longitude">
                            <ItemTemplate>
                                <asp:Label ID="lblLongitude" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Longitude") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="longitudeDataGridFooter" />
                            <FooterStyle CssClass="longitudeDataGridFooter" />
                            <HeaderStyle CssClass="longitudeDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Location Type" SortExpression="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" CssClass="datagridLables" Text='<%# DataBinder.Eval(Container, "DataItem.Type", "{0:d}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Comments">
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedComments" runat="server" CssClass="Normal" /><br />
                                <asp:Label ID="lblWaitingComments" runat="server" CssClass="Normal"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkApproved" runat="server" />
                            </ItemTemplate>
                            <ItemStyle CssClass="typeDataGridFooter" />
                            <FooterStyle CssClass="typeDataGridFooter" />
                            <HeaderStyle CssClass="typeDataGridFooter" />
                        </asp:TemplateColumn>
                        <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>
                    </Columns>
                    <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <AlternatingItemStyle BackColor="#eeeeee" />
                    <ItemStyle BackColor="#f8f8f8" ForeColor="Black" />
                </asp:DataGrid>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div id="div_navigation">
        <asp:ImageButton ID="btnAccept" runat="server" Text="Accept" CssClass="CommandButton" OnClick="btnAccept_Click" Visible="False" ToolTip="Click here to Approve the selected locations." ImageUrl="~/desktopmodules/EngageLocator/images/approve.gif" />
        <asp:ImageButton ID="btnReject" runat="server" Text="Reject" CssClass="CommandButton" OnClick="btnReject_Click" Visible="False" ToolTip="Click here to Reject the selected locations." ImageUrl="~/desktopmodules/EngageLocator/images/reject.gif" />
        <asp:ImageButton ID="btnCancel" runat="server" CssClass="CommandButton" Text="Cancel" OnClick="btnCancel_Click" ToolTip="Click here to go back to the previous screen." ImageUrl="~/desktopmodules/EngageLocator/images/back.gif"/>        
    </div>
    
</div>
