using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Oblivion
{
    public class ObjectiveHUD
    {
        SpriteFont fontStyle;
        Vector2 GoalPosition;
        Vector2 TaskPosition;

        string goal;
        string toDisplay;
        public ObjectiveHUD(ContentManager Content)
        {
            LoadContent(Content);
        }

        public void LoadContent(ContentManager Content)
        {
            fontStyle = Content.Load<SpriteFont>("Fonts/Blade Stroke");
            GoalPosition = new Vector2(40, 150);
            TaskPosition = new Vector2(60, 190);
            goal = "Quest: Duty of the Bound";
        }

        public void Draw(SpriteBatch _spritebatch, int NumOfEnemies)
        {
            toDisplay = NumOfEnemies == 0 ?
                "- Reach the Portal and Defeat Final Boss" : "- Defeat Enemies: " + NumOfEnemies + " Left";

            _spritebatch.DrawString(fontStyle, goal, GoalPosition, Color.Gold, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            _spritebatch.DrawString(fontStyle, toDisplay, TaskPosition, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
        }
    }
}