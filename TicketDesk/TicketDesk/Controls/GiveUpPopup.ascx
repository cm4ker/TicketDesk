﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GiveUpPopup.ascx.cs"
    Inherits="TicketDesk.Controls.GiveUpPopup" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:Button ID="ShowGiveUpButton" CausesValidation="false" runat="server" Text="Give up!" />
<ajaxToolkit:ModalPopupExtender BehaviorID="giveUpBH" ID="GiveUpModalPopupExtender"
    runat="server" TargetControlID="ShowGiveUpButton" PopupControlID="GiveUpPanel"
    BackgroundCssClass="ModalBackground" CancelControlID="CancelGiveUpButton" DropShadow="true"
    PopupDragHandleControlID="GiveUpPanelHandle" />
<asp:Panel ID="GiveUpPanel" runat="server" CssClass="ModalPopup" Style="display: none;">
    <div style="border: solid 1px #A0A0A0;">
        <asp:Panel ID="GiveUpPanelHandle" runat="server" Style="cursor: move; border-bottom: solid 1px #A0A0A0;">
            <div class="ModalPopupHandle">
                Do you want to give up on this ticket (ticket will become unassigned)?
            </div>
        </asp:Panel>
        <div style="padding: 5px;">
            <br />
            Comments (optional):<br />
            <asp:TextBox ID="CommentsTextBox" ValidationGroup="GiveUpPopup" TextMode="MultiLine"
                Rows="5" runat="server" Width="100%" />
            <br />
            <asp:Button ID="GiveUpButton" ValidationGroup="GiveUpPopup" OnClick="GiveUpButton_Click"
                runat="server" Text="Give up!" />
            <asp:Button ID="CancelGiveUpButton" CausesValidation="false" ValidationGroup="GiveUpPopup"
                runat="server" Text="Nevermind" />
        </div>
    </div>
</asp:Panel>
