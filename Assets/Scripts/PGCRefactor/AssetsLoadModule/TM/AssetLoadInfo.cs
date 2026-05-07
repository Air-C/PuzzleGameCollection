namespace PGCRefactor.ModuleAsset.Model
{
    public class AssetLoadInfo
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public bool IsLoaded { get; set; } = false;
    }
}