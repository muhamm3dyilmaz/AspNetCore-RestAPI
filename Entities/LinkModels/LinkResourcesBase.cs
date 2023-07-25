namespace Entities.LinkModels
{
    public class LinkResourcesBase
    {
        //Serialize yapacağımız için const kullandık
        public LinkResourcesBase()
        {
            
        }

        //Birden fazla linkimiz olacağı için listeleme yapıyoruz
        public List<Link> Links { get; set;} = new List<Link>();
    }
}
