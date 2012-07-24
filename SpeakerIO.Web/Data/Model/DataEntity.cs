using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;

namespace SpeakerIO.Web.Data.Model
{
    public abstract class DataEntity
    {
        [Key]
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DataEntity;

            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            bool typesMatch = GetUnproxiedType() == other.GetUnproxiedType();

            bool idsMatch = PersistentAndSame(other);

            return typesMatch && idsMatch;
        }

        public override int GetHashCode()
        {
            // ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
            // http://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=VS.110).aspx
            if (IsTransient()) return base.GetHashCode();
            // ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
            unchecked
            {
                return (GetUnproxiedType().GetHashCode()*31) ^ Id.GetHashCode();
            }
        }

        public static bool operator ==(DataEntity left, DataEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataEntity left, DataEntity right)
        {
            return !Equals(left, right);
        }

        bool PersistentAndSame(DataEntity compareTo)
        {
            return (!Id.Equals(default(long))) &&
                   (!compareTo.Id.Equals(default(long))) && Id.Equals(compareTo.Id);
        }

        bool IsTransient()
        {
            return Id == default(long);
        }

        public Type GetUnproxiedType()
        {
            return ObjectContext.GetObjectType(GetType());
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", GetUnproxiedType().Name, Id);
        }
    }
}