﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;
using System.Collections.Generic;

namespace TicketDesk.Controls
{
    public partial class AssignPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(TicketToDisplay.AssignedTo))
            {
                ShowAssignButton.Text = "Assign";
            }
            else
            {
                ShowAssignButton.Text = "Re-Assign";
            }
            ShowAssignButton.Visible = (SecurityManager.IsStaffOrAdmin && TicketToDisplay.CurrentStatus != "Closed");

            SetPriorityPanel.Visible = string.IsNullOrEmpty(TicketToDisplay.Priority);
        }
        public event TicketPropertyChangedDelegate AssignedToChanged;
        private Ticket _ticket;
        public Ticket TicketToDisplay
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if(!Page.IsPostBack)
            {

            }
        }

        private void BuildUserList()
        {
            AssignDropDownList.SelectedIndex = -1;
            AssignDropDownList.Items.Clear();
            AssignDropDownList.Items.Add(new ListItem("-- select --", "-"));
            AssignDropDownList.Items.AddRange(GetUserList());
        }


        public ListItem[] GetUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetUsersInRoleType("HelpDeskStaffRoleName");
            foreach(User user in users)
            {
                if(user.Name.ToUpperInvariant() != HttpContext.Current.User.Identity.Name.ToUpperInvariant())
                {
                    if(TicketToDisplay.AssignedTo == null || user.Name.ToUpperInvariant() != TicketToDisplay.AssignedTo.ToUpperInvariant())
                    {
                        returnUsers.Add(new ListItem(user.DisplayName,user.Name));
                    }
                }
            }
            return returnUsers.ToArray();
        }


        protected void AssignButton_Click(object sender, EventArgs e)
        {
            string oldAssigned = TicketToDisplay.AssignedTo;
            TicketToDisplay.AssignedTo = AssignDropDownList.SelectedValue;

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            bool setPriority = SetPriorityPanel.Visible && PriorityList.SelectedIndex > -1;
            if(setPriority)
            {
                TicketToDisplay.Priority = PriorityList.SelectedValue;
            }

            if(!string.IsNullOrEmpty(oldAssigned) && oldAssigned.ToUpperInvariant() != Page.User.Identity.GetFormattedUserName().ToUpperInvariant())
            {
                comment.CommentEvent = string.Format("reassigned the ticket from {0} to {1}", SecurityManager.GetUserDisplayName(oldAssigned), SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));
            }
            else if(!string.IsNullOrEmpty(oldAssigned))
            {
                comment.CommentEvent = string.Format("passed the ticket to {0}", SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));

            }
            else
            {
                comment.CommentEvent = string.Format("assigned the ticket to {0}", SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));
            }

            if(setPriority)
            {
                comment.CommentEvent = string.Format("{0} at a priority of {1}", comment.CommentEvent, TicketToDisplay.Priority);
            }
            

           
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = CommentsTextBox.Text.Trim();
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }
            TicketToDisplay.TicketComments.Add(comment);

            AssignModalPopupExtender.Hide();
            if(AssignedToChanged != null)
            {
                AssignedToChanged();
            }

        }

        protected void ShowAssignButton_Click(object sender, EventArgs e)
        {
            BuildUserList();
            AssignModalPopupExtender.Show();
            AssignDropDownList.Focus();
        }
    }
}