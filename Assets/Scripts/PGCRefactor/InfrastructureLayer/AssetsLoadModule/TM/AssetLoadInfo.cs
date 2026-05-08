namespace PGCRefactor.InfrastructureLayer.AssetsLoadModule.Model
{
    public class AssetLoadInfo
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public bool IsLoaded { get; set; } = false;
    }
}