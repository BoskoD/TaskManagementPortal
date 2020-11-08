﻿using Microsoft.Azure.Cosmos.Table;

namespace TaskPortalApi.Models
{
    public class ProjectEntity : TableEntity
    {
        public ProjectEntity()
        {

        }

        public string Description { get; set; }
        public string Code { get; set; }
    }
}