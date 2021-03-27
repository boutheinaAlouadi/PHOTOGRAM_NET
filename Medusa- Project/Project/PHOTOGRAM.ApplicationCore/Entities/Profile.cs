﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace PHOTOGRAM.ApplicationCore.Entities
{
    public class Profile : BaseEntity
    {
        private ICollection<Post> _posts;
        private ICollection<Replay> _replays;
        private ICollection<Like> _likes;
        private ICollection<Follow> _followers;
        private ICollection<Follow> _following;

        public Profile()
        {
        }

        private ILazyLoader LazyLoader { get; set; }

        public Profile(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public byte[] Image { get; set; }

        public ICollection<Replay> Replays
        {
            get => this.LazyLoader.Load(this, ref _replays);
            set => _replays = value;
        }

        public ICollection<Like> Likes
        {
            get => this.LazyLoader.Load(this, ref _likes);
            set => _likes = value;
        }

        public ICollection<Follow> Followers
        {
            get => this.LazyLoader.Load(this, ref _followers);
            set => _followers = value;
        }

        public ICollection<Follow> Following
        {
            get => this.LazyLoader.Load(this, ref _following);
            set => _following = value;
        }

        public ICollection<Post> Posts
        {
            get => this.LazyLoader.Load(this, ref _posts);
            set => _posts = value;
        }
    }
}
