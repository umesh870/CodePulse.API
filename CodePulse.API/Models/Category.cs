﻿namespace CodePulse.API.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string urlHandler { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
