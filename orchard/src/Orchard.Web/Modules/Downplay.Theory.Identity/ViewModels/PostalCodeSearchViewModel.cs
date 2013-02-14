namespace Downplay.Theory.Identity.ViewModels {
    public class PostalCodeSearchViewModel
    {
        public string Query { get; set; }
        public int TotalItemCount { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public dynamic ContentItems { get; set; }
        public dynamic Pager { get; set; }
    }
}