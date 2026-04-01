namespace PGC.ModelEvent.Data
{
    public struct BackGroundMusicSwitchEvent
    {
        public bool switchOn;

        public BackGroundMusicSwitchEvent(bool switchOn)
        {
            this.switchOn = switchOn;
        }
    }
}