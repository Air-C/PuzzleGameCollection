using System.Collections.Generic;
using Entities;

namespace PGC.ModuleClearLine.Model
{
    public class WaitForDestroySquares
    {
        public List<SquareEntity> squares = new ();
        public bool isRender;
        public bool isScored;
        
        public bool AllIsCheck()
        {
            return isRender && isScored;
        }
    }
    

}