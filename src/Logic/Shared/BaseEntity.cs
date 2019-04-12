using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Shared
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is BaseEntity other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }

     
    }
}
