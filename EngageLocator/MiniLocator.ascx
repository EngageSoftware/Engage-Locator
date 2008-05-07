<%@ Control Language="C#" Inherits="Engage.Dnn.Locator.MiniLocator" AutoEventWireup="true" Codebehind="MiniLocator.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>


                        
<script type="text/javascript">            

var curleft = curtop = 0;
var tempTimeout = null;
var curEvent = 0;
var IE = document.all?true:false;
var tempX = 0;
var tempY = 0;


    function Stop() 
    {
        if(tempTimeout) 
        {
            clearTimeout(tempTimeout);
            tempTimeout  = 0;   
        }
    }
    
    function ShowEventInfo(e)
    {
        var	divMap = document.getElementById('<%=divMap.ClientID%>')
        var	pnlAddress = document.getElementById('<%=pnlAddress.ClientID%>')

            Stop();
            
            divMap.style.top = pnlAddress.style.top + 200 + "px";
            divMap.style.left = pnlAddress.style.top + 200 + "px";
            divMap.style.position="absolute";
            divMap.style.display="";
            divMap.style.display="inline";
            divMap.style.zIndex="1"
            divMap.style.visibility="visible";
}
</script>





<asp:UpdatePanel ID="uPnlMiniLocator" runat="server">
    <ContentTemplate>

<table align="left">
<tr>
<td>
    
    </td></tr>

                        <asp:Panel ID="pnlZipCode" runat="server" CssClass="groupingPanel">
                            

                            <tr>
                            <td style="height: 78px; width: 180px;">
                                <asp:Label ID="lblZipCode" runat="server" CssClass="locatorInputLabel" AssociatedControlID="txtZipCode">Please Enter Zip Code:</asp:Label>
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="locatorInput" MaxLength="10" />
                                <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="lnkSubmit_Click" CssClass="SubHead">Find Location</asp:LinkButton>
                            </td></tr>
                        </asp:Panel>
    
                        <div ID="pnlAddress" runat="server" class="pnlAddress">
                            <div id="dvMapImage" runat="server" class="dvMapImage">
                                <asp:Image ID="Image1" runat="server" Height="40px" ImageUrl="..\..\images\icon_maps.gif"
                                Width="48px" />
                            </div>
                            <div id="dvAddressContent" runat="server"  class="dvAddressContent">
                                <asp:Label ID="lblLocationName" runat="server" Text="Label"></asp:Label><br />
                                <asp:Label ID="lblAddress" runat="server" Text="noteTable"></asp:Label><br />
                                <asp:Label ID="lblLocation" runat="server" Text="Label"></asp:Label><br />
                                <asp:LinkButton ID="lnkLocator" runat="server" OnClick="lnklocator_Click">Change Location</asp:LinkButton>
                            </div>
                        </div>
                    </table> 
                     
                     <div runat="server" id="divMap" class="locatorMapSmall">
                        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>

                        

                        
                        
                        
