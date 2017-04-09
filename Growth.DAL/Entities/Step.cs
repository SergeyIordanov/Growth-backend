namespace Growth.DAL.Entities
{
    public class Step : BaseType
    {
        public string Text { get; set; }

        public bool Completed { get; set; }

        public override string CollectionName => "Steps";
    }
}