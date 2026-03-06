
using Custom.Tool;

namespace Controller
{
    public class ShapeManager
    {
        private (string shapeName,Int2[] squareIndex)[] shapeDatas = 
        {
            ("IShape",new Int2[4]{    
                new (-1,0),
                new (0,0),
                new (1,0),
                new (2,0),}
            ),
            ("JShape",new Int2[4]
            {
                new (-1,0),
                new (0,0),
                new (1,0),
                new (-1,1),
            }),
            ("LShape",new Int2[4]
            {
                new (-1,0),
                new (0,0),
                new (1,0),
                new (1,1),
            }),
            ("OShape",new Int2[4]
            {
                new (0,0),
                new (1,0),
                new (0,1),
                new (1,1),
            }),
            ("SShape",new Int2[4]
            {
                new (-1,0),
                new (0,0),
                new (0,1),
                new (1,1),
            }),
            ("TShape",new Int2[4]
            {
                new (-1,0),
                new (0,0),
                new (1,0),
                new (0,1),
            }),
            ("ZShape",new Int2[4]
            {
                new (0,0),
                new (1,0),
                new (-1,1),
                new (0,1),
            }),
        };


        public (string shapeName,Int2[] squareIndex) GetRandomShape()
        {
            int shapeIndex = ConstantTool.GetRandomInt(0, shapeDatas.Length);
            return (shapeDatas[shapeIndex].shapeName,shapeDatas[shapeIndex].squareIndex);
        }
        

    }
}