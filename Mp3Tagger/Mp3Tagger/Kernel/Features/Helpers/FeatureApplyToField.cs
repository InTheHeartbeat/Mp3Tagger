namespace Mp3Tagger.Kernel.Features.Helpers
{
    public class FeatureApplyToField
    {
        public string FieldName { get; set; }
        public bool IsApply { get; set; }

        public FeatureApplyToField()
        {
            FieldName = "name";
            IsApply = false;
        }
    }
}
