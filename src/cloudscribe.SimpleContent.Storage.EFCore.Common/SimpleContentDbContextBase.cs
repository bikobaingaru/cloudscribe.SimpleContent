﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-06-27
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class SimpleContentDbContextBase : DbContext
    {
        public SimpleContentDbContextBase(DbContextOptions options):base(options)
        {

        }

        protected SimpleContentDbContextBase() { }

        public DbSet<ProjectSettings> Projects { get; set; }

        public DbSet<PostEntity> Posts { get; set; }

        public DbSet<PostComment> Comments { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<PageEntity> Pages { get; set; }

        public DbSet<PageComment> PageComments { get; set; }

        public DbSet<PageResourceEntity> PageResources { get; set; }

        public DbSet<PageCategory> PageCategories { get; set; }

        public DbSet<ContentHistory> ContentHistory { get; set; }

    }
}
