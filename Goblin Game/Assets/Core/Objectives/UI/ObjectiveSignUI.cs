using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectiveSignUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshPro topText;
    [SerializeField] TextMeshPro middleText;
    [SerializeField] TextMeshPro bottomText;


    public void Show(Objective objective)
    {
        animator.SetTrigger("show");
        topText.text = "OBJECTIVE:";
        middleText.text = objective.ObjectiveName;
        bottomText.text = objective.Conditions.First().GetPanelDisplay();
    }

    public void UpdateBottomText(ObjectiveCondition condition)
    {
        bottomText.text = condition.GetPanelDisplay();
    }

    public void Hide()
    {
        animator.SetTrigger("hide");
    }
}
