namespace PGC.ModuleScore
{
    public class ScoreSystem
    {
        private GameContext ctx;
        public ScoreSystem(GameContext ctx)
        {
            this.ctx = ctx;
        }

        public void ScoreClearSquare()
        {
            if (ctx.squaresWaitForDestroy.squares.Count == 0 || ctx.squaresWaitForDestroy.isScored)
            {
                return;
            }
            foreach (var square in ctx.squaresWaitForDestroy.squares)
            {
                ctx.totalScore += square.Score;
            }
            ctx.squaresWaitForDestroy.isScored = true;
        }
    }
}