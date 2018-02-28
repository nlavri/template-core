namespace TemplateCore.Core
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public class Manifest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<string> Files { get; set; }
        
        public IList<string> Tokens { get; set; }
    }
}