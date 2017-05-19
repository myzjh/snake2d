using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	// This script will tell you which direction you swiped in
	public class SwipeDirection4 : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			// Hook into the events we need
			LeanTouch.OnFingerSwipe += OnFingerSwipe;
		}
	
		protected virtual void OnDisable()
		{
			// Unhook the events
			LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		}
	
		public void OnFingerSwipe(LeanFinger finger)
		{
			
			// Store the swipe delta in a temp variable
			var swipe = finger.SwipeScreenDelta;
		
			if (swipe.x < -Mathf.Abs(swipe.y) && Head.stepX != Head.step)
			{
				//"You swiped left!";
				Head.stepX = -Head.step;
				Head.stepY = 0;
			}
		
			if (swipe.x > Mathf.Abs(swipe.y) && Head.stepX != -Head.step)
			{
				//"You swiped right!";
				Head.stepX = Head.step;
				Head.stepY = 0;
			}
		
			if (swipe.y < -Mathf.Abs(swipe.x) && Head.stepY != Head.step)
			{
				//"You swiped down!";
				Head.stepX = 0;
				Head.stepY = -Head.step;
			}
		
			if (swipe.y > Mathf.Abs(swipe.x) && Head.stepY != -Head.step)
			{
				//"You swiped up!";
				Head.stepX = 0;
				Head.stepY = Head.step;
			}
		}

	}
}