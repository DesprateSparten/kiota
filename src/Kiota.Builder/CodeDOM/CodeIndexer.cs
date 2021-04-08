﻿namespace Kiota.Builder
{
    public class CodeIndexer : CodeTerminal
    {
        public CodeIndexer(CodeElement parent): base(parent) {
            
        }
        public CodeTypeBase IndexType;
        public CodeTypeBase ReturnType;
    }
}