namespace TemplateCore.Core
{
    public class Package
    {
        public Package()
        {
            this.Manifest = new Manifest();
        }

        public string Path { get; set; }

        public Manifest Manifest { get; set; }
    }
}