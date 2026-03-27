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
            if (ctx.SquaresWaitForDestory.squares.Count == 0 || ctx.SquaresWaitForDestory.isScored)
            {
                return;
            }
            foreach (var square in ctx.SquaresWaitForDestory.squares)
            {
                ctx.totalScore += square.Score;
            }
            ctx.SquaresWaitForDestory.isScored = true;
        }
    }
}