namespace JoshMkhariPROG7312Game.Logic
{
    public class ColoursModel
    
    {
        public byte[][] ColourDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        
        public ColoursModel()
        {
            byte[] Reds = {108,240, 176,112,254,129,39,181,30,88};
            byte[] Green = {71,145, 198,168,202,129,51,71,136,51};
            byte[] Blues= { 34,60,83,188,80,129,73,106,109,84};
            
            ColourDefaults = new byte[3][];
            ColourDefaults[0] = Reds;
            ColourDefaults[1] = Green;
            ColourDefaults[2] = Blues;
        }
    }
}