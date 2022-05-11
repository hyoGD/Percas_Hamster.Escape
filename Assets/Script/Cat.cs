using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using System;

public class Cat : MonoBehaviour
{
    public static Cat instance;
    [Header("SPINE")]
    //  public SkeletonAnimation hamsterSpine;
    public SkeletonAnimation cat;
    [SpineAnimation] public List<string> catAnimation;
    [SpineSkin] public string[] skinCat, glasses, hats;
   
    private void Awake()
    {
        {
            instance = this;
        }
    }

    private void Start()
    {

      //  ChangeSkin(cat, skinCat[3],glasses[3], hats[3] );
    }
    public static void ChangeSkin(SkeletonAnimation skAnim, string ssSkinChange,string glasse, string hat)
    {
        var skeleton = skAnim.Skeleton;
        var skeletonData = skeleton.Data;
        var newSkin = new Skin("new-skin");

        newSkin.AddSkin(skeletonData.FindSkin(ssSkinChange));
        //newSkin.AddSkin(skeletonData.FindSkin(glasse));
        //newSkin.AddSkin(skeletonData.FindSkin(hat));
        skAnim.skeleton.SetSkin(newSkin);
        skeleton.SetSlotsToSetupPose();
        skAnim.AnimationState.Apply(skeleton);
    }

    public void PlayAnimCat(int num)
    {

        switch (num)
        {
            case 0:
                cat.AnimationState.SetAnimation(0,catAnimation[num], true);
                break;
            case 1:
                cat.AnimationState.SetAnimation(0, catAnimation[num], true);
                break;
            default:
                cat.AnimationState.SetAnimation(0, catAnimation[num], false);
                break;
        } 
    }

    }
