﻿using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace _8x8.Impls
{
    public abstract class BaseFilterRuleStrategy<T> : Disposable, IFilterRuleStrategy<T>
    {
        public BaseFilterRuleStrategy(IFilterRule filterRule)
        {
            FilterRule = filterRule;
            Accesor = TypeAccessor.Create(filterRule.GetType());
        }

        protected TypeAccessor Accesor { get; }

        public virtual IFilterRule FilterRule { get; protected set; }

        public virtual IEnumerable<string> Segments { get; protected set; }

        public virtual T Hash { get; protected set; }

        public abstract bool Equals([AllowNull] IFilterRuleStrategy<T> other);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();

        protected IEnumerable<PropertyInfo> GetInheritancePropertiesFromIFilterRule(IFilterRule filterRule)
        {
            Type typeFilterRule = typeof(IFilterRule);
            var types = filterRule.GetType().GetInterfaces().Where(c => typeFilterRule.IsAssignableFrom(c) && !typeFilterRule.Equals(c)).ToArray();
            foreach (var type in types)
            {
                foreach (var pi in type.GetProperties())
                {
                    yield return pi;
                }
            }
        }
    }
}