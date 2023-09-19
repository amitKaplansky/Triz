using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Hitcode_RoomEscape
{
    public class ButtonClick : MonoBehaviour
    {

        // Use this for initialization
        Counter counterscript;
        Actions[] actions;
        void Start()
        {

           
            actions = transform.parent.GetComponents<Actions>();
            counterscript = transform.parent.GetComponent<Counter>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool islocked = false;

        private void OnMouseUpAsButton()
        {

            if (islocked)
                return;

            Vector3 tsize = transform.localScale;

            transform.DOScale(tsize * 1.2f, .2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                islocked = false;
                transform.localScale = tsize;
            });

            islocked = true;


            if (counterscript.getNumber() == counterscript.correctNumber)
            {

                foreach(Actions taction in actions)
                {
                    for(int i = 0; i < taction.actionSteps.Count; i++)
                    {
                        Debug.Log(taction.actionSteps.Count);
                        taction.playActionNow(i);
                    }
                   
                }

            }
            else
            {
                print("wrong");
                GameManager.getInstance().playSfx("wrong");
            }
        }
    }
}
