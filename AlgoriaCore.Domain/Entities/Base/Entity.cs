using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AlgoriaCore.Domain.Entities.Base
{
    [Serializable]
    public abstract class Entity : Entity<int>, IEntity
    {
    }

    [Serializable]
    public abstract class Entity<TType> : IEntity<TType>
        //where TType : struct
    {
        /// <summary>
        /// Identificador único para esta entidad.
        /// </summary>
        public virtual TType Id { get; set; }

        /// <summary>
        /// Verifica si esta entidad es transitoria (no tiene un Id).
        /// </summary>
        /// <returns>Verdadero, si es entidad es transitoria</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TType>.Default.Equals(Id, default(TType)))
            {
                return true;
            }

            //Solución alterna para EF Core ya que establece el valor int/long al valor mínimo cuando se adjunta al dbcontext
            if (typeof(TType) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TType) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TType>))
            {
                return false;
            }

            //Algunas instancias deben ser consideradas como iguales
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Objetos transitorios no son considerados iguales
            var other = (Entity<TType>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            //Debe tener una relación de tipos IS-A o debe ser del mismo tipo
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }
			
			if (MayHaveTenant() && other.MayHaveTenant() &&
				(this as IMayHaveTenant).TenantId != (other as IMayHaveTenant).TenantId) 
            {
                return false;
            }

            if (MustHaveTenant() && other.MustHaveTenant() &&
                (this as IMustHaveTenant).TenantId != (other as IMustHaveTenant).TenantId)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }

            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }

        public bool MustHaveTenant()
        {
			return this.GetType() is IMayHaveTenant;
        }

        public bool MayHaveTenant()
        {
            return this.GetType() is IMustHaveTenant;
        }
    }
}
