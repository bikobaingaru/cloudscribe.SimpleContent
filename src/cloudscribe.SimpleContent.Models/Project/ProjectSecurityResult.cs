﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2018-10-23
// 


namespace cloudscribe.SimpleContent.Models
{
    public class ProjectSecurityResult
    {
        public ProjectSecurityResult(
            string displayName,
            string projectId,
            bool isAuthenticated, 
            bool canEditPosts,
            bool canEditPages,
            string timeZoneId = "GMT"
            )
        {
            this.displayName = displayName;
            this.projectId = projectId;
            this.isAuthenticated = isAuthenticated;
            this.canEditPosts = canEditPosts;
            this.canEditPages = canEditPages;
            this.timeZoneId = timeZoneId;
        }

        private string displayName = string.Empty;
        private string projectId = string.Empty;
        private bool isAuthenticated = false;
        private bool canEditPosts = false;
        private bool canEditPages = false;
        private string timeZoneId = "GMT";

        public string DisplayName
        {
            get { return displayName; }
        }

        public string ProjectId
        {
            get { return projectId; }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public bool CanEditPosts
        {
            get { return canEditPosts; }
        }

        public bool CanEditPages
        {
            get { return canEditPages; }
        }

        public string TimeZoneId
        {
            get { return timeZoneId; }
        }
    }
}
