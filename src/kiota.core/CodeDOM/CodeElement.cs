﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace kiota.core
{
    /// <summary>
    /// Abstract element of some piece of source code to be generated
    /// </summary>
    public abstract class CodeElement
    {
        public CodeElement(CodeElement parent)
        {
            if(parent == null && !(this is CodeNamespace))
                throw new ArgumentNullException(nameof(parent));
            Parent = parent;
        }
        public CodeElement Parent { get; set; }
        public abstract IList<CodeElement> GetChildElements();

        public abstract string Name
        {
            get; set;
        }

        public void Render(LanguageWriter writer)
        {
            writer.Write(this);
        }
        protected void AddMissingParent(params CodeElement[] elements) {
            foreach(var element in elements.Where(x => x.Parent == null))
                element.Parent = this;
        }
        public T GetImmediateParentOfType<T>(CodeElement item = null) {
            if(item == null)
                return GetImmediateParentOfType<T>(this);
            else if (item is T p)
                return p;
            else if (item.Parent == null)
                throw new InvalidOperationException($"item {item.Name} of type {item.GetType()} does not have a parent");
            else if(item.Parent is T p2)
                return p2;
            else
                return GetImmediateParentOfType<T>(item.Parent);
        }
    }
}