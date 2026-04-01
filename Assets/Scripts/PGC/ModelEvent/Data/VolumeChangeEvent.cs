namespace PGC.ModelEvent.Data
{
    public struct VolumeChangeEvent
    {
        public float volume;

        public VolumeChangeEvent(float volume)
        {
            this.volume = volume;
        }
    }
}