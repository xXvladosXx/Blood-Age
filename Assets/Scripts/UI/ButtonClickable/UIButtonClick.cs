namespace DefaultNamespace.UI.ButtonClickable
{
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIButtonClick : MonoBehaviour
    {
        public void WasClicked(GameObject gameObuttonbject)
        {
            print("WWW");
            if (!GetComponent<SkillTree>().SatisfiesRequirements(gameObuttonbject.GetComponent<SkillHolder>())) return;

            gameObuttonbject.GetComponent<SkillHolder>().Active = true;
            GetComponent<SkillTree>().UpdateSkills(gameObuttonbject.GetComponent<SkillHolder>());
            gameObuttonbject.GetComponent<Image>().enabled = true;

            print(gameObuttonbject + " was clicked");
        }
    }
}