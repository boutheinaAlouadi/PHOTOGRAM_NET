﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace PHOTOGRAM.ApplicationCore.Entities
{
    public class Follow : BaseEntity
    {
        private Profile _follower;
        private Profile _following;

        public Follow()
        {
        }

        private ILazyLoader LazyLoader { get; set; }

        public Follow(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        public Guid? FollowerId { get; set; }
        public Profile Follower
        {
            get => this.LazyLoader.Load(this, ref _follower);
            set => _follower = value;
        }

        public Guid? FollowingId { get; set; }
        public Profile Following
        {
            get => this.LazyLoader.Load(this, ref _following);
            set => _following = value;
        }
    }
}
